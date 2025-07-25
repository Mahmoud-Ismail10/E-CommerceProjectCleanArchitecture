using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Emails.Commands.Models
{
    public record SendEmailCommand(string Email, string ReturnUrl, EmailType EmailType) : IRequest<ApiResponse<string>>;
}
