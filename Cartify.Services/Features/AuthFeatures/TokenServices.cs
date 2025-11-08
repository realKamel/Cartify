using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cartify.Domain.Exceptions;
using Cartify.Services.Abstractions;
using Cartify.Services.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cartify.Services.Features.AuthFeatures;

public class TokenServices(IOptions<JwtConfigurationOptions> jwtOptions) : ITokenServices
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(jwtOptions.Value.SecurityKey));

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var singInCredential =
            new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var tokenOptions = new JwtSecurityToken(issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationInMinutes),
            signingCredentials: singInCredential);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken)
    {
        if (accessToken is null)
        {
            throw new InvalidTokenException("Invalid token");
        }

        //Must Check if the token isn't manipulated
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = _key,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha512],
            ValidateLifetime = false,
            ValidateIssuer = true,
            ValidateAudience = true
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(accessToken, validationParameters, out var token);

        if (token is null)
        {
            throw new InvalidTokenException("Invalid token");
        }

        return principal;
    }

    public void StoreTokensInsideCookies(string accessToken, string refreshToken, HttpContext context)
    {
        context.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationInMinutes)
        });

        context.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays)
        });
    }
}