using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.AuthService.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Service.AuthService.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Helper
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        #endregion

        #region Constructors
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion

        #region Functions
        public Guid GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims?.SingleOrDefault(claim => claim.Type == nameof(UserClaimModel.Id))?.Value;
            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("UnAuthenticated");
            return Guid.Parse(userId);
        }

        public Guid GetCartOwnerId()
        {
            // If the user is authenticated, return their UserId
            if (IsAuthenticated) return GetUserId();

            // If the user is not authenticated, generate or retrieve a GuestId
            var guestId = _httpContextAccessor.HttpContext?.Request.Cookies["GuestId"];
            if (string.IsNullOrEmpty(guestId) || !Guid.TryParse(guestId, out var parsedGuestId))
            {
                parsedGuestId = Guid.NewGuid();
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("GuestId", parsedGuestId.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });
            }

            return parsedGuestId;
        }

        public async Task<User> GetUserAsync()
        {
            Guid userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new UnauthorizedAccessException();
            return user;
        }

        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public bool DeleteGuestIdCookie()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    if (httpContext.Request.Cookies.ContainsKey("GuestId"))
                    {
                        httpContext.Response.Cookies.Delete("GuestId");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
