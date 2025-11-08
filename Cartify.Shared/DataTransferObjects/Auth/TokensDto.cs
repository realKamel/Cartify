namespace Cartify.Shared.DataTransferObjects.Auth;

public record TokensDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}