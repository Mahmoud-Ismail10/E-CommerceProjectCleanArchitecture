using AutoMapper;

namespace E_Commerce.Core.Mapping.Employees
{
    public partial class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            AddEmployeeCommandMapping();
            EditEmployeeCommandMapping();
            GetEmployeeByIdQueryMapping();
        }
    }
}
