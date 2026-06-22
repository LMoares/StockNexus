using StockNexusAPI.Application.Common.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace StockNexusAPI.Infrastructure.Services.UserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var principal = _httpContextAccessor.HttpContext?.User;
                var claimValue =
                    principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ??
                    principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                return int.TryParse(claimValue, out var userId) ? userId : null;
            }
        }

        public string? Email => 
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                ??
            _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
