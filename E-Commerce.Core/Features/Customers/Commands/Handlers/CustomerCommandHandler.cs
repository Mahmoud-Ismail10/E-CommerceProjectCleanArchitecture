using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Customers.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Customers.Commands.Handlers
{
    internal class CustomerCommandHandler : ApiResponseHandler,
        IRequestHandler<EditCustomerCommand, ApiResponse<string>>,
        IRequestHandler<DeleteCustomerCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CustomerCommandHandler(UserManager<User> userManager, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            var oldCustomer = await _userManager.FindByIdAsync(request.Id.ToString());
            if (oldCustomer is null) return NotFound<string>();

            var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName, request.Id);
            if (isUserNameDuplicate)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);

            var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email, request.Id);
            if (isEmailDuplicate)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]);

            var newCustomer = _mapper.Map(request, oldCustomer);
            var updateResult = await _userManager.UpdateAsync(newCustomer);

            if (!updateResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            return Edit("");
        }

        public async Task<ApiResponse<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _userManager.FindByIdAsync(request.Id.ToString());
            if (customer is null) return NotFound<string>();

            var deleteResult = await _userManager.DeleteAsync(customer);
            if (!deleteResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
            return Deleted<string>();
        }
        #endregion
    }
}
