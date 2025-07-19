using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Security
{
    public static class AuthorizationConstants
    {
        public const string AdminPolicy = "AdminPolicy";
        public const string AdminRole = "AdminRole";

        public const string EmployeePolicy = "EmployeePolicy";
        public const string EmployeeRole = "EmployeeRole";

        public const string CustomerPolicy = "CustomerPolicy";
        public const string CustomerRole = "CustomerRole";
    }
}
