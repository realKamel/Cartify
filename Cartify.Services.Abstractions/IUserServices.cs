using Cartify.Shared.DataTransferObjects.Auth;
using Microsoft.AspNetCore.Http;

namespace Cartify.Services.Abstractions;

public interface IUserServices
{
    //Must Be Admin
    public Task<IReadOnlyList<UserDataResponseDto>> GetAllUsers(bool noTracking = true,
        CancellationToken cancellationToken = default);

    public Task<UserDataResponseDto> GetUserById(string userId, CancellationToken cancellationToken = default);
    
    public Task DeleteUser(string email, CancellationToken cancellationToken = default);

    public Task<LoggedInUserResponseDto> LogIn(UserLogInRequestDto request, HttpContext context,
        CancellationToken cancellationToken = default);

    public Task RegisterUser(UserRegistrationRequestDto userDto, HttpContext context,
        CancellationToken cancellationToken = default);

    public Task UpdateUserPersonalInfo(HttpContext context, UserUpdatePersonalDataRequestDto userDto,
        CancellationToken cancellationToken = default);

    public Task UpdateUserPassword(HttpContext context, LoggedInUserUpdatePasswordRequestDto userDto,
        CancellationToken cancellationToken = default);

    public Task RefreshAccessToken(HttpContext context);

    public Task Logout(HttpContext context);
}