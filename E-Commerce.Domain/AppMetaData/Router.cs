namespace E_Commerce.Domain.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "{id}";

        public const string Root = "api";
        public const string Version = "v1";
        public const string Rule = Root + "/" + Version + "/";

        public static class EmailsRoute
        {
            public const string Prefix = Rule + "email/";
            public const string SendEmail = Prefix + "sendEmail";
        }

        public static class Authentication
        {
            public const string Prefix = Rule + "authenticate/";
            public const string SignIn = Prefix + "signIn";
            public const string RefreshToken = Prefix + "refreshToken";
            public const string ValidateToken = Prefix + "validateToken";
            public const string SendResetPasswordCode = Prefix + "sendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "confirmResetPasswordCode";
            public const string ResetPassword = Prefix + "resetPassword";
            public const string ConfirmEmail = "api/authenticate/confirmEmail";
        }

        public static class Authorization
        {
            public const string Prefix = Rule + "authorize/";
            public const string Role = Prefix + "role/";
            public const string Claim = Prefix + "claim/";
            public const string CreateRole = Role + "create";
            public const string EditRole = Role + "edit";
            public const string DeleteRole = Role + SingleRoute;
            public const string GetAllRoles = Role + "getAll";
            public const string GetRoleById = Role + SingleRoute;
            public const string ManageUserRoles = Role + "manageUserRoles/" + SingleRoute;
            public const string UpdateUserRoles = Role + "updateUserRoles";
            public const string ManageUserClaims = Claim + "manageUserClaims/" + SingleRoute;
            public const string UpdateUserClaims = Claim + "updateUserClaims";
        }

        public static class UserRouting
        {
            public const string Prefix = Rule + "user/";
            public const string Register = Prefix + "register";
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

        public static class OrderRouting
        {
            public const string Prefix = Rule + "order/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CartRouting
        {
            public const string Prefix = Rule + "cart/";
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

        public static class ShippingAddressRouting
        {
            public const string Prefix = Rule + "shippingAddress/";
            public const string GetAll = Prefix + "getAll";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }
    }
}
