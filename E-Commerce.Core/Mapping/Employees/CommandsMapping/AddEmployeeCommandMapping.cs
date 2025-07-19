using E_Commerce.Core.Features.Employees.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Employees
{
    public partial class EmployeeProfile
    {
        public void AddEmployeeCommandMapping()
        {
            CreateMap<AddEmployeeCommand, Employee>();
        }
    }
}
