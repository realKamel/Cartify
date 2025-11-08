using System.ComponentModel.DataAnnotations;

namespace Cartify.Shared.DataTransferObjects.Auth;

public record UserLogInRequestDto
{
    [Required] [EmailAddress] public string Email { get; init; }

    [Required] public string Password { get; init; }
}