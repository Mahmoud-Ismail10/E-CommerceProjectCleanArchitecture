using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Employees.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace E_Commerce.Core.Features.Employees.Commands.Handlers
{
    public class EmployeeCommandHandler : ApiResponseHandler,
        IRequestHandler<AddEmployeeCommand, ApiResponse<string>>,
        IRequestHandler<EditEmployeeCommand, ApiResponse<string>>,
        IRequestHandler<DeleteEmployeeCommand, ApiResponse<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public EmployeeCommandHandler(UserManager<User> userManager, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ApiResponse<string>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]);

            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);

            var identityUser = _mapper.Map<Employee>(request);
            var createResult = await _userManager.CreateAsync(identityUser, request.Password);
            if (!createResult.Succeeded)
                return BadRequest<string>(createResult.Errors.FirstOrDefault().Description);

            //Add default role "Employee"
            var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, "Employee");
            if (!addToRoleResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]);

            //Add default employee policies
            var claims = new List<Claim>
            {
                new Claim("Edit Employee", "True"),
                new Claim("Get Employee", "True")
            };
            var addDefaultClaimsResult = await _userManager.AddClaimsAsync(identityUser, claims);
            if (!addDefaultClaimsResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]);

            return Created("");
        }

        public async Task<ApiResponse<string>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            var oldEmployee = await _userManager.FindByIdAsync(request.Id.ToString());
            if (oldEmployee is null) return NotFound<string>();

            var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName, request.Id);
            if (isUserNameDuplicate)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);

            var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email, request.Id);
            if (isEmailDuplicate)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]);

            var newEmployee = _mapper.Map(request, oldEmployee);
            var updateResult = await _userManager.UpdateAsync(newEmployee);

            if (!updateResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            return Edit("");
        }

        public async Task<ApiResponse<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var customer = await _userManager.FindByIdAsync(request.Id.ToString());
            if (customer is null) return NotFound<string>();

            var deleteResult = await _userManager.DeleteAsync(customer);
            if (!deleteResult.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
            return Deleted<string>();
        }
    }
}
