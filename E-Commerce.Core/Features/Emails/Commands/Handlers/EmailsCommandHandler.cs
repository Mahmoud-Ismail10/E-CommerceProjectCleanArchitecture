using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Emails.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler : ApiResponseHandler,
        IRequestHandler<SendEmailCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IEmailsService _emailsService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EmailsCommandHandler(IEmailsService emailsService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _emailsService = emailsService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmailAsync(request.Email, request.ReturnUrl, request.EmailType);
            if (response == "Success")
                return Success("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]);
        }
        #endregion
    }
}
