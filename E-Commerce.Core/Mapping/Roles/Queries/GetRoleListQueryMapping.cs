using E_Commerce.Core.Features.Authorization.Queries.Responses;
using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRoleListQueryMapping()
        {
            CreateMap<Role, GetRoleListResponse>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
