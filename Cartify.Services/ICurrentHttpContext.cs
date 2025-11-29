using Microsoft.AspNetCore.Http;

namespace Cartify.Services
{
    public interface ICurrentHttpContext
    {
        string? IpAddress { get; }
        HttpResponse? Response { get; }
        HttpRequest? Request { get; }
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        IEnumerable<string> Roles { get; }
        string? GetClaim(string claimType);
        string? Path { get; }
        string? Method { get; }
        IHeaderDictionary? Headers { get; }
    }
}
