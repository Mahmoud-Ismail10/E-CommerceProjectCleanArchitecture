namespace E_Commerce.Domain.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "{id}";

        public const string Root = "api";
        public const string Version = "v1";
        public const string Rule = Root + "/" + Version + "/";

        public static class Authentication
        {
            public const string Prefix = Rule + "authenticate/";
            public const string Register = Prefix + "register";
            public const string SignIn = Prefix + "signIn";
            public const string RefreshToken = Prefix + "refreshToken";
            public const string ValidateToken = Prefix + "validateToken";
        }

        public static class Authorization
        {
            public const string Prefix = Rule + "authorize/";
            public const string CreateRole = Prefix + "role/create";
            public const string EditRole = Prefix + "role/edit";
            public const string DeleteRole = Prefix + "role/" + SingleRoute;
            public const string GetAllRoles = Prefix + "role/getAll";
            public const string GetRoleById = Prefix + "role/" + SingleRoute;
        }

        public static class UserRouting
        {
            public const string Prefix = Rule + "user/";
            public const string ChangePassword = Prefix + "changePassword";
        }

        public static class CategoryRouting
        {
            public const string Prefix = Rule + "category/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ProductRouting
        {
            public const string Prefix = Rule + "product/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CustomerRouting
        {
            public const string Prefix = Rule + "customer/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class EmployeeRouting
        {
            public const string Prefix = Rule + "employee/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }
    }
}
