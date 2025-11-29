namespace Cartify.Shared.DataTransferObjects.Auth;

public record UserDataResponseDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public bool LockoutEnabled { get; set; }
};