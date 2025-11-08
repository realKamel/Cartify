using System.ComponentModel.DataAnnotations;

namespace Cartify.Shared.DataTransferObjects.Auth;

public record UserRegistrationRequestDto
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required]
    [StringLength(265, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 265 characters")]
    public required string Name { get; set; }

    [Required]
    [RegularExpression("/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/")]
    public required string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
    public required string RePassword { get; set; }

    [Required]
    [RegularExpression("/^01[0125]\\d{8}$/")]
    public required string Phone { get; set; }
}