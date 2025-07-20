using AutoMapper;

namespace E_Commerce.Core.Mapping.Roles
{
    public partial class RoleProfile : Profile
    {
        public RoleProfile()
        {
            GetRoleByIdQueryMapping();
            GetRoleListQueryMapping();
        }
    }
}
