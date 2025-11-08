using Cartify.Domain.Entities;
using Cartify.Shared.DataTransferObjects.Auth;

namespace Cartify.Services.Features.AuthFeatures;

public static class AuthHelper
{
    public static AppUser ToEntity(this UserRegistrationRequestDto userDto, string refreshToken,
        int refreshTokenExpiryTimeInDays)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        return new AppUser
        {
            Email = userDto.Email,
            PhoneNumber = userDto.Phone,
            Name = userDto.Name,
            UserName = userDto.Email,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryTimeInDays)
        };
    }

    public static LoggedInUserResponseDto ToSignedInUserDto(this AppUser user, List<string> roles)
    {
        return new LoggedInUserResponseDto
        {
            Email = user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
            Name = user.Name ?? throw new ArgumentNullException(nameof(user.UserName)),
            Roles = roles
        };
    }

    public static UserDataResponseDto ToUserDataResponseDto(this AppUser user)
    {
        return new UserDataResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
            UserName = user.UserName ?? throw new ArgumentNullException(nameof(user.UserName)),
            PhoneNumber = user.PhoneNumber, //throw new ArgumentNullException(nameof(user.PhoneNumber)),
            EmailConfirmed = user.EmailConfirmed,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            AccessFailedCount = user.AccessFailedCount,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
        };
    }
}