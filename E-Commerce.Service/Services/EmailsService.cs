using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.Services.Contract;
using MailKit.Net.Smtp;
using MimeKit;

namespace E_Commerce.Service.Services
{
    public class EmailsService : IEmailsService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        #endregion

        #region Constructors
        public EmailsService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        #endregion

        #region Handle Functions
        public async Task<string> SendEmailAsync(string email, string ReturnUrl, EmailType? emailType)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    // (1) Start connection with host and send port
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    // (2) Send Credentials
                    client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $@"
                           <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                               <div style='max-width: 600px; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); margin: auto;'>
                                   <h2 style='color: #333;'>{(emailType == EmailType.ConfirmEmail ? "Confirm Your Email" : "Reset Your Password")}</h2>
                                   <p style='color: #555; font-size: 16px;'>
                                       {(emailType == EmailType.ConfirmEmail
                                           ? "Thank you for signing up! Please confirm your email by clicking the button below."
                                           : "We received a request to reset your password. Click the button below to proceed.")}
                                   </p>
                                   <a href='{ReturnUrl}' style='display: inline-block; background-color: #007bff; color: white; text-decoration: none; padding: 12px 20px; border-radius: 5px; font-size: 16px; margin-top: 20px;'>
                                       {(emailType == EmailType.ConfirmEmail ? "Confirm Email" : "Reset Password")}
                                   </a>
                                   <p style='color: #999; font-size: 14px; margin-top: 20px;'>
                                       If you didn't request this, you can safely ignore this email.
                                   </p>
                               </div>
                           </div>
                        ",

                        TextBody = emailType switch
                        {
                            EmailType.ConfirmEmail => $"Thank you for signing up! Please confirm your email using this link: {ReturnUrl}",
                            EmailType.ForgotPassword => $"We received a request to reset your password. Use this link to proceed: {ReturnUrl}",
                            _ => "Please confirm your email using the provided link."
                        }
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("E-Commerce Support", _emailSettings.FromEmail)); // Display Name, Email Address of source
                    message.To.Add(new MailboxAddress("Testing", email)); // Email Address of destination
                    message.Subject = emailType switch // The text that will be pressed (Provided Link)
                    {
                        EmailType.ConfirmEmail => "Confirm Your Email",
                        EmailType.ForgotPassword => "Reset Password",
                        _ => "No Submitted"
                    };
                    // (3) Send content of message
                    await client.SendAsync(message);
                    // (4) Disconnect from the server
                    await client.DisconnectAsync(true);
                }
                //End of sending email
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
        #endregion
    }
}
