using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Cartify.Shared.DataTransferObjects.Auth;

/// <summary>
/// DTO for User Login.
/// </summary>
[SwaggerSchema(Description = "User login data transfer object.")]
public record UserLogInRequestDto
{
    /// <summary>
    /// User's Registered Email 
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    /// User's Password
    /// </summary>
    [Required]
    public required string Password { get; init; }
}