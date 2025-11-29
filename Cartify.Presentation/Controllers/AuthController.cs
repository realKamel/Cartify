using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;


/// <summary>
/// Provides API endpoints for user authentication and account management, including login, registration, password
/// updates, profile changes, and administrative user queries.
/// </summary>
/// <remarks>All endpoints are versioned and require appropriate authorization. Administrative actions, such as
/// retrieving user lists or individual user data, are restricted to users with the Admin role. Endpoints for login,
/// registration, password updates, and profile changes are available to authenticated users as appropriate. The
/// controller delegates business logic to the provided user services implementation.</remarks>
/// <param name="services">The user services implementation used to perform authentication, registration, and user management operations.</param>
public class AuthController(IUserServices services) : V1BaseController
{


    /// <summary>
    /// Retrieves a list of all users in the system. Accessible only to users with the 'Admin' role.
    /// </summary>
    /// <remarks>This endpoint requires authentication and 'Admin' role authorization. The returned list
    /// includes all users currently registered in the system.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An HTTP 200 response containing a list of user data if successful. Returns 401 if the caller is unauthorized, or
    /// 403 if the caller does not have the required role.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    [ProducesResponseType(typeof(List<UserDataResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<UserDataResponseDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await services.GetAllUsers(cancellationToken: cancellationToken);
        return Ok(result);
    }




    /// <summary>
    /// Retrieves detailed information about a user specified by the user ID.
    /// </summary>
    /// <remarks>Requires the caller to have the 'Admin' role. Returns 401 Unauthorized if the caller is not
    /// authenticated, or 403 Forbidden if the caller lacks sufficient permissions.</remarks>
    /// <param name="userId">The unique identifier of the user to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult{UserDataResponseDto}"/> containing the user's data if found; returns a 404 Not Found
    /// response if the user does not exist.</returns>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDataResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDataResponseDto>> GetUser([FromRoute] string userId,
        CancellationToken cancellationToken = default)
    {
        var result = await services.GetUserById(userId, cancellationToken);
        return Ok(result);
    }





    /// <summary>
    /// Authenticates a user using the provided login credentials and returns user information if authentication is
    /// successful.
    /// </summary>
    /// <remarks>This endpoint expects a JSON payload and responds with user information upon successful
    /// login. If authentication fails or the request is invalid, an appropriate error response is returned. The
    /// operation can be cancelled using the provided cancellation token.</remarks>
    /// <param name="requestDto">An object containing the user's login credentials, such as username and password. Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the login operation.</param>
    /// <returns>An ActionResult containing a LoggedInUserResponseDto with user details if authentication succeeds; returns 400
    /// Bad Request if the request is invalid, or 401 Unauthorized if authentication fails.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoggedInUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes("application/json")]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [Produces("application/json")]
    public async Task<ActionResult<LoggedInUserResponseDto>> Login(UserLogInRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var result = await services.LogIn(requestDto, cancellationToken);
        return Ok(result);
    }



    /// <summary>
    /// Registers a new user account using the provided registration details.
    /// </summary>
    /// <remarks>This endpoint creates a new user account. If the registration details are invalid or the user
    /// already exists, the appropriate HTTP status code is returned. The operation can be cancelled using the provided
    /// cancellation token.</remarks>
    /// <param name="requestDto">An object containing the user's registration information, such as username, password, and contact details.
    /// Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the registration operation.</param>
    /// <returns>A result indicating the outcome of the registration request. Returns 201 Created if registration is successful,
    /// 400 Bad Request if the input is invalid, or 409 Conflict if the user already exists.</returns>
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterUser(UserRegistrationRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.RegisterUser(requestDto, cancellationToken);
        return Created();
    }


    /// <summary>
    /// Endpoint to logout user and revoke the tokens
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> SignOut(CancellationToken cancellationToken)
    {
        await services.Logout();
        return NoContent();
    }



    /// <summary>
    /// Updates the password for the currently authenticated user.
    /// </summary>
    /// <remarks>This action requires the user to be authenticated. The request will fail if the provided
    /// password information is invalid or if authentication is not successful.</remarks>
    /// <param name="requestDto">An object containing the current and new password information required to update the user's password.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the password update operation.</param>
    /// <returns>A result indicating the outcome of the password update request. Returns a 204 No Content response if the
    /// password is successfully updated; otherwise, returns an appropriate error response.</returns>
    [HttpPatch("password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUserPassword(LoggedInUserUpdatePasswordRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateUserPassword(requestDto, cancellationToken);
        return NoContent();
    }




    /// <summary>
    /// Updates the authenticated user's personal profile information with the provided data.
    /// </summary>
    /// <remarks>This endpoint requires authentication. Only the currently authenticated user's profile can be
    /// updated. Validation errors in the request data will result in a 400 Bad Request response.</remarks>
    /// <param name="requestDto">An object containing the new personal information to update for the user. All required fields must be valid;
    /// otherwise, the request will fail.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the update operation.</param>
    /// <returns>An HTTP 200 OK response if the update is successful; otherwise, an appropriate error response such as 400 Bad
    /// Request for invalid input or 401 Unauthorized if the user is not authenticated.</returns>
    [HttpPatch("profile")]
    [Authorize]
    [ProducesResponseType(typeof(UserDataResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUserPersonalInfo(UserUpdatePersonalDataRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateUserPersonalInfo(requestDto, cancellationToken);
        return Ok();
    }





    /// <summary>
    /// Refreshes the access token for the currently authenticated user.
    /// </summary>
    /// <remarks>This endpoint requires authentication. The refreshed token is returned in the response
    /// according to the application's authentication scheme.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the refresh operation.</param>
    /// <returns>An HTTP 200 OK response if the token is successfully refreshed; otherwise, an HTTP 401 Unauthorized response if
    /// the user is not authenticated.</returns>
    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(LoggedInUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        await services.RefreshAccessToken();
        return Ok();
    }
}