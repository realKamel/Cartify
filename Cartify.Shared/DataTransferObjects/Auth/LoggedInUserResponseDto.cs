namespace Cartify.Shared.DataTransferObjects.Auth;

public record LoggedInUserResponseDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<string> Roles { get; set; }
}