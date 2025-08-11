using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;

namespace E_Commerce.Service.Services
{
    public class PaymobService : IPaymobService
    {
        #region Fields
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymobCashInBroker _broker;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IEmailsService _emailsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly PaymobSettings _paymobSettings;
        #endregion

        #region Constructors
        public PaymobService(IPaymentRepository paymentRepository,
            IPaymobCashInBroker broker,
            ICartService cartService,
            IProductService productService,
            IOrderService orderService,
            IEmailsService emailsService,
            ICurrentUserService currentUserService,
            PaymobSettings paymobSettings)
        {
            _paymentRepository = paymentRepository;
            _broker = broker;
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _emailsService = emailsService;
            _currentUserService = currentUserService;
            _paymobSettings = paymobSettings;
        }
        #endregion

        #region Private Helpers
        private string GetCartKey() => $"cart:{_currentUserService.GetCartOwnerId()}";
        #endregion

        #region Handle Functions
        // Create a payment request in Paymob by the user's cart information
        public async Task<string> CreateOrUpdatePaymentAsync(Guid cartId)
        {
            using var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                // Get cart from repository
                var cartKey = GetCartKey();
                var cart = await _cartService.GetCartByKeyAsync(cartKey);

                if (cart is null || !cart.CartItems.Any())
                    return "CartIsEmptyOrDoesNotExist";

                // Update cart item prices from current product prices
                foreach (var item in cart.CartItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                        item.Price = product.Price;
                }

                // Convert amount to cents as required by Paymob (100 piasters = 1 EGP)
                var amountCents = (int)(cart.TotalAmount! * 100);

                // Create Paymob order
                var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
                var orderResponse = await _broker.CreateOrderAsync(orderRequest);

                // Add payment intent ID to the cart for tracking
                cart.PaymentIntentId = orderResponse.Id.ToString();

                // Update cart in repository
                var result = await _cartService.AddOrEditCartAsync(cart);
                if (result is null)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error updating cart: {Message}", result);
                    return "FailedToUpdateOrderAfterPaymentCreation";
                }
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error adding order: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToCreatePaymentOrder";
            }
        }

        public async Task<string> ProcessPaymentForOrderAsync(Guid orderId)
        {
            using var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                // Find the order
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order is null) return "OrderIdNotFound";

                // Get the shipping information if exist (for billing data)
                var shippingAddress = order.ShippingAddress;

                // Get customer information
                var customer = order.Customer;
                if (customer is null) return "NoCustomerFoundForOrder";

                // Prepare amount in cents
                var amountCents = (int)(order.TotalAmount * 100 ?? 0);

                // Create Paymob order
                var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
                var orderResponse = await _broker.CreateOrderAsync(orderRequest);

                // Get customer name parts (first name, last name)
                string firstName = customer.FirstName ?? "Guest";
                string lastName = customer.LastName ?? "User";

                // Create billing data from shipping information
                var billingData = new CashInBillingData(
                    firstName: firstName,
                    lastName: lastName,
                    phoneNumber: customer.PhoneNumber,
                    email: customer.Email,
                    country: "N/A",
                    state: shippingAddress.State,
                    city: shippingAddress.City,
                    apartment: "N/A",
                    street: shippingAddress.Street,
                    floor: "N/A",
                    building: "N/A",
                    shippingMethod: order.Delivery.DeliveryMethod.ToString(),
                    postalCode: "N/A");

                // Get integration ID from configuration
                if (!int.TryParse(_paymobSettings.IntegrationId, out int integrationId))
                    return "InvalidPaymobIntegrationIDInConfiguration";

                // Create payment key request
                var paymentKeyRequest = new CashInPaymentKeyRequest
                (
                    integrationId: integrationId,
                    orderId: orderResponse.Id,
                    billingData: billingData,
                    amountCents: amountCents,
                    currency: "EGP",
                    lockOrderWhenPaid: true,
                    expiration: null
                );

                // Request payment key from Paymob
                var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);

                // Create a new payment record
                var payment = new Payment
                {
                    OrderId = order.Id,
                    TransactionId = orderResponse.Id.ToString(),
                    TotalAmount = order.TotalAmount,
                    PaymentDate = DateTimeOffset.UtcNow.ToLocalTime(),
                    PaymentMethod = PaymentMethod.Paymob,
                    Status = Status.Pending
                };

                // Add payment record to database
                await _paymentRepository.AddAsync(payment);

                // Update order status
                order.Status = Status.Pending;

                // Store payment token for iframe URL
                order.PaymentToken = paymentKeyResponse.PaymentKey;

                // Save order changes
                var result = await _orderService.EditOrderAsync(order);
                if (result != "Success")
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error updating order after payment creation: {Message}", result);
                    return "FailedToUpdateOrderAfterPaymentCreation";
                }
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error adding order: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToProcessPaymentForOrder";
            }
        }

        public string GetPaymentIframeUrl(string paymentToken)
        {
            if (string.IsNullOrEmpty(paymentToken))
                return "PaymentTokenCannotBeNullOrEmpty";

            string iframeId = _paymobSettings.IframeId;
            if (string.IsNullOrEmpty(iframeId))
                return "PaymobIframeIDIsNotConfigured";

            // Build the Paymob iframe URL
            string iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentToken}";
            return iframeUrl;
        }

        public async Task<string> UpdateOrderSuccessAsync(string paymentIntentId)
        {
            using var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                // Find the payment record by transaction ID
                var payment = await _paymentRepository.GetPaymentByTransactionId(paymentIntentId);
                if (payment is null)
                {
                    // Try to find by order ID as a fallback
                    payment = await _paymentRepository.GetPaymentByOrderId(Guid.Parse(paymentIntentId));
                    if (payment is null) return "PaymentIdNotFound";
                }

                // Find the order linked to this payment
                var order = await _orderService.GetOrderByIdAsync(payment.OrderId);
                if (order is null) return "OrderIdNotFound";

                // Update order status
                order.Status = Status.Completed;

                // Update delivery status if needed
                if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                    order.Delivery!.Status = Status.Completed;

                // Update payment record
                payment.Status = Status.Completed;
                payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();

                // Save changes
                var result1 = await _cartService.DeleteCartAsync(order.CustomerId);
                if (!result1)
                    return "FailedToDeleteCartAfterOrderSuccess";

                var result2 = await _orderService.EditOrderAsync(order);
                if (result2 != "Success")
                {
                    await transaction.RollbackAsync();
                    return "FailedToUpdateOrderSuccess";
                }

                await _paymentRepository.UpdateAsync(payment);

                // Send success email to customer
                var result3 = await _emailsService.SendEmailAsync(null!, null!, EmailType.OrderConfirmation);
                if (result3 != "Success")
                {
                    await transaction.RollbackAsync();
                    return "FailedToSendOrderConfirmationEmail";
                }
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error updating order success: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToUpdateOrderSuccess";
            }
        }

        public async Task<string> UpdateOrderFailed(string paymentIntentId)
        {
            using var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                // Find the payment record by transaction ID
                var payment = await _paymentRepository.GetPaymentByTransactionId(paymentIntentId);
                if (payment is null)
                {
                    // Try to find by order ID as a fallback
                    payment = await _paymentRepository.GetPaymentByOrderId(Guid.Parse(paymentIntentId));
                    if (payment is null) return "PaymentIdNotFound";
                }

                // Find the order linked to this payment
                var order = await _orderService.GetOrderByIdAsync(payment.OrderId);
                if (order is null) return "OrderIdNotFound";

                // Update order status
                order.Status = Status.Failed;

                // Update shipping status if needed
                if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                    order.Delivery!.Status = Status.Completed;

                // Update payment record
                payment.Status = Status.Failed;
                payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();

                // Update delivery status if needed
                if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                    order.Delivery!.Status = Status.Failed;

                // Save changes
                var result = await _orderService.EditOrderAsync(order);
                if (result != "Success")
                    return "FailedToUpdateOrderFailed";

                await _paymentRepository.UpdateAsync(payment);

                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error updating order failed: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToUpdateOrderFailed";
            }
        }

        public async Task<string> ProcessTransactionCallback(CashInCallbackTransaction callback)
        {
            if (callback is null)
                return "CallbackDataIsNull";

            // Log callback data for debugging - fixing the incorrect string interpolation
            Log.Information($"Received transaction callback for ID: {callback.Id}");

            // The CashInCallbackTransaction structure has changed - it no longer has a Transaction property
            // Instead, we need to use the properties directly from the callback object

            var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                // Find the payment record by transaction ID from Paymob
                var payment = await _paymentRepository.GetPaymentByTransactionId(callback.Id.ToString());
                if (payment is null)
                {
                    // Try to find by order ID if available
                    var orderId = Guid.Parse(callback.Order.Id.ToString());
                    payment = await _paymentRepository.GetPaymentByOrderId(orderId);
                    if (payment is null)
                    {
                        // If still not found, create a new payment record if we can find the order
                        var order = await _orderService.GetOrderByIdAsync(Guid.Parse(callback.Order.Id.ToString()));
                        if (order is null)
                            return "NoOrderFoundWithInCallback";

                        payment = new Payment
                        {
                            Id = order.Id,
                            TransactionId = callback.Id.ToString(),
                            TotalAmount = (decimal)(callback.AmountCents / 100.0), // Convert back from cents
                            PaymentDate = DateTime.Now,
                            PaymentMethod = PaymentMethod.Paymob,
                            Status = Status.Pending,
                        };
                        await _paymentRepository.AddAsync(payment);
                    }
                }

                // Find the order linked to this payment
                var orderToUpdate = await _orderService.GetOrderByIdAsync(payment.OrderId);
                if (orderToUpdate is null)
                    return "NoOrderLinkedToThisPayment";

                // Update order and payment status based on the transaction callback
                switch (callback.ApiSource)
                {
                    case "IFRAME":
                    case "INVOICE":
                        // Process transaction status
                        if (callback.Success && !callback.IsVoided && !callback.IsRefunded)
                        {
                            // Payment successful
                            orderToUpdate.Status = Status.Paid;
                            payment.Status = Status.Received;

                            // Update delivery status if needed
                            if (orderToUpdate.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                                orderToUpdate.Delivery!.Status = Status.Pending;
                        }
                        else if (callback.IsRefunded)
                        {
                            // Payment refunded
                            orderToUpdate.Status = Status.Pending;
                            payment.Status = Status.Refunded;
                        }
                        else if (callback.IsVoided)
                        {
                            // Payment voided
                            orderToUpdate.Status = Status.Pending;
                            payment.Status = Status.Voided;
                        }
                        else
                        {
                            // Payment failed
                            orderToUpdate.Status = Status.Pending;
                            payment.Status = Status.Failed;
                        }
                        break;

                    default:
                        // Unknown callback type, log but don't change status
                        Log.Warning($"Unhandled API source: {callback.ApiSource}");
                        break;
                }

                payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();

                // Save changes
                var result = await _orderService.EditOrderAsync(orderToUpdate);
                if (result != "Success")
                {
                    await transaction.RollbackAsync();
                    return "FailedToUpdateOrderAfterPaymentCallback";
                }
                await _paymentRepository.UpdateAsync(payment);
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error processing transaction callback: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToProcessTransactionCallback";
            }
        }

        public string ComputeHmacSHA512(string data, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hash = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        #endregion
    }
}
