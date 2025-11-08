using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Cartify.Services.Abstractions;

public interface ITokenServices
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken);
    public void StoreTokensInsideCookies(string accessToken, string refreshToken, HttpContext context);
}