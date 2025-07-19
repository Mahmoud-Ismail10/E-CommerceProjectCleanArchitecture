using E_Commerce.Core.Features.Employees.Queries.Responses;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Employees
{
    public partial class EmployeeProfile
    {
        public void GetEmployeeByIdQueryMapping()
        {
            CreateMap<Employee, GetSingleEmployeeResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
