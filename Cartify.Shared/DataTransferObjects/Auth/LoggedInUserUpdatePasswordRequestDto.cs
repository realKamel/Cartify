using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cartify.Shared.DataTransferObjects.Auth;

public record LoggedInUserUpdatePasswordRequestDto
{
    [Required]
    [JsonPropertyName("currentPassword")]
    public required string CurrentPassword { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [RegularExpression("/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/")]
    public required string NewPassword { get; set; }

    [Required]
    [JsonPropertyName("rePassword")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords must match")]
    public required string ConfirmNewPassword { get; set; }
}