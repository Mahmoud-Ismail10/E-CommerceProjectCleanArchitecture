using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authorization.Queries.Models;
using E_Commerce.Core.Features.Authorization.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Responses;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ApiResponseHandler,
        IRequestHandler<GetRoleByIdQuery, ApiResponse<GetSingleRoleResponse>>,
        IRequestHandler<GetRoleListQuery, ApiResponse<List<GetRoleListResponse>>>,
        IRequestHandler<ManageUserRolesQuery, ApiResponse<ManageUserRolesResponse>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public RoleQueryHandler(IAuthorizationService authorizationService, UserManager<User> userManager, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<GetSingleRoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIdAsync(request.Id);
            if (role is null) NotFound<GetSingleRoleResponse>();
            var roleMapper = _mapper.Map<GetSingleRoleResponse>(role);
            return Success(roleMapper);
        }

        public async Task<ApiResponse<List<GetRoleListResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var roleList = await _authorizationService.GetRolesListAsync();
            if (roleList is null) NotFound<GetRoleListResponse>();
            var roleListMapper = _mapper.Map<List<GetRoleListResponse>>(roleList);
            return Success(roleListMapper);
        }

        public async Task<ApiResponse<ManageUserRolesResponse>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) return NotFound<ManageUserRolesResponse>();
            var result = await _authorizationService.ManageUserRolesData(user);
            return Success(result);
        }
        #endregion
    }
}
