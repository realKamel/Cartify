using System.Security.Claims;
using Cartify.Domain.Entities;
using Cartify.Domain.Exceptions;
using Cartify.Services.Abstractions;
using Cartify.Services.Helper;
using Cartify.Shared.DataTransferObjects.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cartify.Services.Features.AuthFeatures;

public class UserServices(
	UserManager<AppUser> userManager,
	ITokenServices tokenServices,
	ICurrentHttpContext context,
	IOptions<JwtConfigurationOptions> jwtOptions,
	ILogger<UserServices> logger) : IUserServices
{
	public async Task<IReadOnlyList<UserDataResponseDto>> GetAllUsers(bool noTracking = true,
		CancellationToken cancellationToken = default)
	{
		var result = userManager.Users.AsQueryable();
		if (noTracking)
		{
			result = result.AsNoTracking();
		}

		var usersDto = await result
			.Select(e => e.ToUserDataResponseDto()).ToListAsync(cancellationToken);
		return usersDto;
	}


	public async Task DeleteUser(string email, CancellationToken cancellationToken = default)
	{
		var appUserExists = await userManager.FindByEmailAsync(email);

		if (appUserExists is null)
		{
			throw new UserNotFoundException("User is not found");
		}

		await userManager.DeleteAsync(appUserExists);
	}

	public async Task<LoggedInUserResponseDto> LogIn(UserLogInRequestDto requestDto, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByEmailAsync(requestDto.Email);

		if (user is null)
		{
			throw new UserNotFoundException("User is not found");
		}

		var isCorrectPassword = await userManager.CheckPasswordAsync(user, requestDto.Password);

		if (!isCorrectPassword)
		{
			throw new CartItemNotFoundException("Incorrect Email or password");
		}

		var roles = await userManager.GetRolesAsync(user);
		var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

		var accessToken = tokenServices.GenerateAccessToken(
		[
			new Claim(ClaimTypes.Email, user.Email!),
			new Claim(ClaimTypes.NameIdentifier, user.Id),
			..userRoles
		]);
		user.RefreshToken = tokenServices.GenerateRefreshToken();
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
		var result = await userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
		}

		var userData = user.ToSignedInUserDto(roles.ToList());

		tokenServices.StoreTokensInsideCookies(accessToken, user.RefreshToken);

		return userData;
	}

	public async Task RegisterUser(UserRegistrationRequestDto userDto,
		CancellationToken cancellationToken = default)
	{
		var isUserEmailExist = await userManager.FindByEmailAsync(userDto.Email);

		if (isUserEmailExist is not null)
		{
			throw new DuplicateEmailException($"{userDto.Email} is already registered");
		}

		var user = userDto.ToEntity(tokenServices.GenerateRefreshToken(),
			jwtOptions.Value.RefreshTokenExpirationInDays);

		var createdUserResult = await userManager.CreateAsync(user);

		if (!createdUserResult.Succeeded)
		{
			logger.LogError("Error in creating user {errors}", createdUserResult.Errors);
			throw new Exception(string.Join("\n", createdUserResult.Errors.Select(e => e.Description)));
		}

		var addToRoleResult = await userManager.AddToRoleAsync(user, "Customer");

		if (!addToRoleResult.Succeeded)
		{
			logger.LogError("Error in creating user {errors}", addToRoleResult.Errors);
			throw new Exception(string.Join("\n", addToRoleResult.Errors.Select(e => e.Description)));
		}

		var accessToken = tokenServices
			.GenerateAccessToken(
			[
				new Claim(ClaimTypes.Email, user.Email!),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Role, "Customer")
			]);

		tokenServices.StoreTokensInsideCookies(accessToken, user.RefreshToken);
	}

	public async Task UpdateUserPersonalInfo(UserUpdatePersonalDataRequestDto userDto,
		CancellationToken cancellationToken = default)
	{
		var accessToken = "";
		context?.Request?.Cookies.TryGetValue("AccessToken", out accessToken);

		if (string.IsNullOrEmpty(accessToken))
		{
			throw new UnauthorizedAccessException("Please Login First");
		}

		var principal = tokenServices.GetPrincipalFromExpiredAccessToken(accessToken);

		var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		var user = await userManager.FindByEmailAsync(email!);

		if (user is null)
		{
			throw new UserNotFoundException("User is not found");
		}

		user.Email = userDto.Email;
		user.UserName = userDto.Name;
		user.PhoneNumber = userDto.Phone;
		var result = await userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			throw new UserAlreadyExistsException("Email or Username already exists");
		}
	}

	public async Task UpdateUserPassword(LoggedInUserUpdatePasswordRequestDto userDto,
		CancellationToken cancellationToken = default)
	{
		var accessToken = "";
		context?.Request?.Cookies.TryGetValue("AccessToken", out accessToken);

		if (string.IsNullOrEmpty(accessToken))
		{
			throw new UnauthorizedAccessException("Please Login First");
		}

		var principal = tokenServices.GetPrincipalFromExpiredAccessToken(accessToken);

		var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		var user = await userManager.FindByEmailAsync(email!);
		if (user is null)
		{
			throw new UserNotFoundException("User is not found");
		}

		var result = await userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.NewPassword);
		if (!result.Succeeded)
		{
			throw new CartItemNotFoundException("Error in changing Password");
		}
	}

	public async Task RefreshAccessToken()
	{
		string? accessToken = "", refreshToken = "";

		context?.Request?.Cookies.TryGetValue("AccessToken", out accessToken);
		context?.Request?.Cookies.TryGetValue("RefreshToken", out refreshToken);

		if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
		{
			throw new UnauthorizedAccessException("Login First");
		}

		var principal = tokenServices.GetPrincipalFromExpiredAccessToken(accessToken);

		var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (string.IsNullOrEmpty(userId))
		{
			throw new UnauthorizedAccessException("User is not Authorized");
		}

		var user = await userManager.FindByIdAsync(userId);

		if (user is null)
		{
			throw new UserNotFoundException("User is not found");
		}

		if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
		{
			throw new UnauthorizedAccessException();
		}

		var newAccessToken = tokenServices.GenerateAccessToken(principal.Claims);
		user.RefreshToken = tokenServices.GenerateRefreshToken();
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
		await userManager.UpdateAsync(user);

		tokenServices.StoreTokensInsideCookies(newAccessToken, user.RefreshToken);
	}

	public async Task Logout()
	{
		ClearAuthCookies();

		// Try to get user ID from access token if present and valid

		string? userId = null, accessToken = "";
		if (
			context?.Request?.Cookies.TryGetValue("AccessToken", out accessToken) ?? false
			&& !string.IsNullOrEmpty(accessToken))
		{
			try
			{
				var principal = tokenServices.GetPrincipalFromExpiredAccessToken(accessToken);
				userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			}
			catch
			{
				// Ignore — token might be malformed, expired beyond recovery, etc.
			}
		}

		// If we have a user ID, invalidate their refresh token in DB
		if (!string.IsNullOrEmpty(userId))
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user is not null && !string.IsNullOrEmpty(user.RefreshToken))
			{
				user.RefreshToken = string.Empty;
				user.RefreshTokenExpiryTime = DateTime.UtcNow;
				await userManager.UpdateAsync(user);
			}
		}
	}

	private void ClearAuthCookies()
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.None,
			Domain = null
		};

		context?.Response?.Cookies.Delete("AccessToken", cookieOptions);
		context?.Response?.Cookies.Delete("RefreshToken", cookieOptions);
	}

	public async Task<UserDataResponseDto> GetUserById(string userId, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException("User is not found");
		var userDto = user.ToUserDataResponseDto();
		return userDto;
	}
}