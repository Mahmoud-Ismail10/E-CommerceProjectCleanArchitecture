using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authorization.Queries.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Responses;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authorization.Queries.Handlers
{
    public class ClaimsQueryHandler : ApiResponseHandler,
        IRequestHandler<ManageUserClaimsQuery, ApiResponse<ManageUserClaimsResponse>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ClaimsQueryHandler(IAuthorizationService authorizationService, UserManager<User> userManager, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<ManageUserClaimsResponse>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) return NotFound<ManageUserClaimsResponse>();
            var result = await _authorizationService.ManageUserClaimsData(user);
            return Success(result);
        }
        #endregion
    }
}
