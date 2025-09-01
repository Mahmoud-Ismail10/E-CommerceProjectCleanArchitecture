using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Service.Services.Contract
{
    public interface IEmailsService
    {
        Task<string> SendEmailAsync(string email, string ReturnUrl, EmailType? emailType, Order? order = null);
    }
}
