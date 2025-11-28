using Microsoft.AspNetCore.Identity;

namespace Cartify.Domain.Entities;

public class AppUser : IdentityUser
{
    public required string Name { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}