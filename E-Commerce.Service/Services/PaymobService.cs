using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Domain.Responses;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;

namespace E_Commerce.Service.Services
{
    public class PaymobService : IPaymobService
    {
        #region Fields
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymobCashInBroker _broker;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailsService _emailsService;
        private readonly PaymobSettings _paymobSettings;
        #endregion

        #region Constructors
        public PaymobService(IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IPaymobCashInBroker broker,
            ICartService cartService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IEmailsService emailsService,
            PaymobSettings paymobSettings)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _broker = broker;
            _cartService = cartService;
            _productService = productService;
            _currentUserService = currentUserService;
            _emailsService = emailsService;
            _paymobSettings = paymobSettings;
        }
        #endregion

        #region Private Helpers
        private string GetCartKey() => $"cart:{_currentUserService.GetCartOwnerId()}";
        #endregion

        #region Handle Functions
        // Create a payment request in Paymob by the user's cart information
        //public async Task<string> CreateOrUpdatePaymentAsync(Guid cartId)
        //{
        //    using var transaction = await _paymentRepository.BeginTransactionAsync();
        //    try
        //    {
        //        // Get cart from repository
        //        var cartKey = GetCartKey();
        //        var cart = await _cartService.GetCartByKeyAsync(cartKey);

        //        if (cart is null || !cart.CartItems.Any())
        //            return "CartIsEmptyOrDoesNotExist";

        //        // Update cart item prices from current product prices
        //        foreach (var item in cart.CartItems)
        //        {
        //            var product = await _productService.GetProductByIdAsync(item.ProductId);
        //            if (product != null)
        //                item.Price = product.Price;
        //        }

        //        // Convert amount to cents as required by Paymob (100 piasters = 1 EGP)
        //        var amountCents = (int)(cart.TotalAmount! * 100);

        //        // Create Paymob order
        //        var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
        //        var orderResponse = await _broker.CreateOrderAsync(orderRequest);

        //        // Add payment intent ID to the cart for tracking
        //        cart.PaymentIntentId = orderResponse.Id.ToString();

        //        // Update cart in repository
        //        var result = await _cartService.AddOrEditCartAsync(cart);
        //        if (result is null)
        //        {
        //            await transaction.RollbackAsync();
        //            Log.Error("Error updating cart: {Message}", result);
        //            return "FailedToUpdateOrderAfterPaymentCreation";
        //        }
        //        await transaction.CommitAsync();
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        Log.Error("Error adding order: {Message}", ex.InnerException?.Message ?? ex.Message);
        //        return "FailedToCreatePaymentOrder";
        //    }
        //}

        public async Task<(Order?, string)> ProcessPaymentForOrderAsync(Order order)
        {
            try
            {
                // Get the shipping information if exist (for billing data)
                var shippingAddress = order.ShippingAddress;

                // Get customer information
                var customer = order.Customer;
                if (customer is null) return (null, "NoCustomerFoundForOrder");

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
                    phoneNumber: customer.PhoneNumber!,
                    email: customer.Email!,
                    country: "N/A",
                    state: shippingAddress!.State!,
                    city: shippingAddress.City!,
                    apartment: "N/A",
                    street: shippingAddress.Street!,
                    floor: "N/A",
                    building: "N/A",
                    shippingMethod: order.Delivery!.DeliveryMethod.ToString()!,
                    postalCode: "N/A");

                // Get integration ID from configuration
                if (!int.TryParse(_paymobSettings.IntegrationId, out int integrationId))
                    return (null, "InvalidPaymobIntegrationIDInConfiguration");

                // Create payment key request
                var paymentKeyRequest = new CashInPaymentKeyRequest
                (
                    integrationId: integrationId,
                    orderId: orderResponse.Id,
                    billingData: billingData,
                    amountCents: amountCents,
                    currency: "EGP",
                    lockOrderWhenPaid: true,
                    expiration: 3600
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
                order.Status = Status.Completed;

                // Store payment token for iframe URL
                order.PaymentToken = paymentKeyResponse.PaymentKey;

                return (order, "Success");
            }
            catch (Exception ex)
            {
                Log.Error("Error in process payment: {Message}", ex.InnerException?.Message ?? ex.Message);
                return (null, "FailedToProcessPaymentForOrder");
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

        private async Task<string> UpdateOrderStatusAsync(string paymentIntentId, Status orderStatus, Status paymentStatus)
        {
            using var transaction = await _paymentRepository.BeginTransactionAsync();
            try
            {
                var payment = await _paymentRepository.GetPaymentByTransactionId(paymentIntentId)
                             ?? await _paymentRepository.GetPaymentByOrderId(Guid.Parse(paymentIntentId));

                if (payment is null) return "PaymentNotFound";

                var order = await _orderRepository.GetByIdAsync(payment.OrderId);

                order.Status = orderStatus;
                if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                    order.Delivery!.Status = orderStatus;

                payment.Status = paymentStatus;
                payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();

                await _orderRepository.UpdateAsync(order);
                await _paymentRepository.UpdateAsync(payment);

                if (orderStatus == Status.Completed)
                {
                    var result1 = await _cartService.DeleteCartAsync(order.CustomerId);
                    if (!result1)
                        return "FailedToDeleteCartAfterOrderSuccess";

                    var result3 = await _emailsService.SendEmailAsync(order.Customer!.Email!, null!, EmailType.OrderConfirmation, order);
                    if (result3 != "Success")
                    {
                        await transaction.RollbackAsync();
                        return "FailedToSendOrderConfirmationEmail";
                    }
                }


                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error updating order: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedToUpdateOrder";
            }
        }

        public async Task<string> ProcessTransactionCallbackAsync(CustomCashInCallbackTransaction callback, Guid orderId)
        {
            Log.Information($"Received transaction callback for ID: {callback.Id}");

            var payment = await _paymentRepository.GetPaymentByTransactionId(callback.Id!.ToString());
            if (payment is null)
            {
                //var orderId = Guid.Parse(callback.Order.Id.ToString());
                payment = await _paymentRepository.GetPaymentByOrderId(orderId);

                if (payment is null)
                {
                    payment = new Payment
                    {
                        Id = orderId,
                        TransactionId = callback.Id.ToString(),
                        TotalAmount = callback.AmountCents / 100.0m,
                        PaymentDate = DateTime.Now,
                        PaymentMethod = PaymentMethod.Paymob,
                        Status = Status.Pending,
                    };
                    await _paymentRepository.AddAsync(payment);
                }
            }

            return callback switch
            {
                { Success: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Completed, Status.Completed),
                { IsRefunded: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Pending, Status.Refunded),
                { IsVoided: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Pending, Status.Voided),
                _ => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Failed, Status.Failed)
            };
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
