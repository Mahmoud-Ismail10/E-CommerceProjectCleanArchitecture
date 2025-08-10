using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class EmailsService : IEmailsService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderService _orderService;
        #endregion

        #region Constructors
        public EmailsService(EmailSettings emailSettings, ICurrentUserService currentUserService, IOrderService orderService)
        {
            _emailSettings = emailSettings;
            _currentUserService = currentUserService;
            _orderService = orderService;
        }
        #endregion

        #region Handle Functions
        public async Task<string> SendEmailAsync(string email, string ReturnUrl, EmailType? emailType)
        {
            var userId = _currentUserService.GetUserId();
            var order = await _orderService.GetLatestOrderForUserAsync(userId);
            var customerEmail = order.Customer?.Email;
            var paymentStatus = order.Payment?.Status?.ToString() ?? "Unknown";

            var statusColor = paymentStatus switch
            {
                "Completed" => "green",
                "Received" => "green",
                "Failed" => "red",
                "Pending" => "orange",
                "Refunded" => "blue",
                _ => "gray"
            };

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = emailType switch
                {
                    EmailType.ConfirmEmail => $@"
                             <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                                 <div style='max-width: 600px; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); margin: auto;'>
                                     <h2 style='color: #333;'>Confirm Your Email</h2>
                                     <p style='color: #555; font-size: 16px;'>
                                         Thank you for signing up! Please confirm your email by clicking the button below.
                                     </p>
                                     <a href='{ReturnUrl}' style='display: inline-block; background-color: #007bff; color: white; text-decoration: none; padding: 12px 20px; border-radius: 5px; font-size: 16px; margin-top: 20px;'>Confirm Email</a>
                                     <p style='color: #999; font-size: 14px; margin-top: 20px;'>
                                         If you didn't request this, you can safely ignore this email.
                                     </p>
                                 </div>
                             </div>",

                    EmailType.ResetPassword => $@"
                             <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                                 <div style='max-width: 600px; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); margin: auto;'>
                                     <h2 style='color: #333;'>Reset Your Password</h2>
                                     <p style='color: #555; font-size: 16px;'>
                                         We received a request to reset your password. Use the code below to proceed.
                                     </p>
                                     <div style='display: inline-block; background-color: #007bff; color: white; padding: 12px 20px; border-radius: 5px; font-size: 20px; margin-top: 20px; font-weight: bold;'>{ReturnUrl}</div>
                                     <p style='color: #999; font-size: 14px; margin-top: 20px;'>
                                         If you didn't request this, you can safely ignore this email.
                                     </p>
                                 </div>
                             </div>",

                    EmailType.OrderConfirmation => $@"
                              <html>
                                  <body style='font-family: Arial, sans-serif; color: #333;'>
                                      <h2>Order Placed Successfully!</h2>
                                      <p>Dear {order.Customer?.FirstName} {order.Customer?.LastName},</p>
                                      <p>Your order #{order.Id} has been successfully placed. We're now preparing your items for delivery or pickup.</p>
                             
                                      <table style='border-collapse: collapse; width: 100%; max-width: 600px;'>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Order ID:</td>
                                              <td style='padding: 8px;'>{order.Id}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Order Date:</td>
                                              <td style='padding: 8px;'>{order.OrderDate:MMMM dd, yyyy}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Payment Status:</td>
                                              <td style='padding: 8px; color: {statusColor};'>{paymentStatus}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Delivery Method:</td>
                                              <td style='padding: 8px;'>{order.Delivery?.DeliveryMethod}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Estimated Delivery Date:</td>
                                              <td style='padding: 8px;'>{order.Delivery?.DeliveryTime:MMMM dd, yyyy}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Order Cost:</td>
                                              <td style='padding: 8px;'>{order.TotalAmount}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Delivery Cost:</td>
                                              <td style='padding: 8px;'>{order.Delivery?.Cost}</td>
                                          </tr>
                                          <tr>
                                              <td style='padding: 8px; font-weight: bold;'>Total Amount:</td>
                                              <td style='padding: 8px;'>{order.TotalAmount + order.Delivery?.Cost:F2}</td>
                                          </tr>
                                      </table>
                             
                                      <p>You'll receive another notification when your order ships or is ready for pickup. If you have any questions, feel free to contact our support team.</p>
                                      <p>Thank you for shopping with us!</p>
                                      <p>Best regards,<br/>The SMarket Team</p>
                                  </body>
                              </html>",

                    _ => "Invalid email type"
                },

                TextBody = emailType switch
                {
                    EmailType.ConfirmEmail => $"Thank you for signing up! Please confirm your email using this link: {ReturnUrl}",
                    EmailType.ResetPassword => $"We received a request to reset your password.\nYour reset code is: {ReturnUrl}",
                    _ => "Please confirm your email using the provided link."
                }
            };
            var message = new MimeMessage();
            message.Body = bodybuilder.ToMessageBody();
            message.From.Add(new MailboxAddress("E-Commerce Support", _emailSettings.FromEmail)); // Display Name, Email Address of source
            message.To.Add(new MailboxAddress("ECommerce", email)); // Email Address of destination
            message.Subject = emailType switch // The text that will be pressed (Provided Link)
            {
                EmailType.ConfirmEmail => "Confirm Your Email",
                EmailType.ResetPassword => "Reset Password",
                EmailType.OrderConfirmation => "Order Placed Confirmation",
                _ => "No Submitted"
            };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error sending email to {Email}: {Message}", email, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }
        #endregion
    }
}
