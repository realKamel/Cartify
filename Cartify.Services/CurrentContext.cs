using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Cartify.Services
{
    internal class CurrentHttpContext(IHttpContextAccessor contextAccessor) : ICurrentHttpContext
    {
        private readonly HttpContext? context = contextAccessor.HttpContext;
        public string? IpAddress
        {
            get
            {
                var forwarded = context?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwarded))
                    return forwarded.Split(',')[0];

                return context?.Connection?.RemoteIpAddress?.ToString();
            }
        }
        public HttpResponse? Response => context?.Response;
        public HttpRequest? Request => context?.Request;
        public string? UserId => context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string? Email => context?.User?.FindFirst(ClaimTypes.Email)?.Value;
        public bool IsAuthenticated => context?.User?.Identity?.IsAuthenticated ?? false;
        public IEnumerable<string> Roles => context?.User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value) ?? [];
        public string? GetClaim(string claimType) => context?.User?.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;

        public string? Path => context?.Request?.Path.Value;
        public string? Method => context?.Request?.Method;
        public IHeaderDictionary? Headers => context?.Request?.Headers;
    }
}
