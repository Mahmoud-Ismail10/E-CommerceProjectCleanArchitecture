using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Commands.Models;
public record ResetPasswordCommand(string Email, string NewPassword, string ConfirmPassword) : IRequest<ApiResponse<string>>;
