using System.ComponentModel.DataAnnotations;

namespace Cartify.Shared.DataTransferObjects.Auth;

/// <summary>
/// Dto For User Registration
/// </summary>
public record UserRegistrationRequestDto
{
    /// <summary>
    /// User’s email address (must be unique).
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }


    /// <summary>
    /// User's Display Name
    /// </summary>
    [Required]
    [StringLength(265, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 265 characters")]
    public required string Name { get; set; }


    /// <summary>
    /// Password for the new account. Must be at least 8 characters.
    /// </summary>
    [Required]
    [RegularExpression("/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/")]
    public required string Password { get; set; }

    /// <summary>
    /// Re-Password for conformation.
    /// </summary>
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
    public required string RePassword { get; set; }


    /// <summary>
    /// User Phone Number
    /// </summary>
    [Required]
    [RegularExpression("/^01[0125]\\d{8}$/")]
    public required string Phone { get; set; }
}