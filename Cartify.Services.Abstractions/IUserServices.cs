using Cartify.Shared.DataTransferObjects.Auth;

namespace Cartify.Services.Abstractions;

public interface IUserServices
{
	//Must Be Admin
	Task<IReadOnlyList<UserDataResponseDto>> GetAllUsers(bool noTracking = true, CancellationToken cancellationToken = default);

	Task<UserDataResponseDto> GetUserById(string userId, CancellationToken cancellationToken = default);

	Task DeleteUser(string email, CancellationToken cancellationToken = default);

	Task<LoggedInUserResponseDto> LogIn(UserLogInRequestDto request, CancellationToken cancellationToken = default);

	Task RegisterUser(UserRegistrationRequestDto userDto, CancellationToken cancellationToken = default);

	Task UpdateUserPersonalInfo(UserUpdatePersonalDataRequestDto userDto, CancellationToken cancellationToken = default);

	Task UpdateUserPassword(LoggedInUserUpdatePasswordRequestDto userDto, CancellationToken cancellationToken = default);

	Task RefreshAccessToken();

	Task Logout();
}