using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

public class AuthController(IUserServices services) : V1BaseController
{
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
        var result = await services.LogIn(requestDto, HttpContext, cancellationToken);
        return Ok(result);
    }


    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterUser(UserRegistrationRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.RegisterUser(requestDto, HttpContext, cancellationToken);
        return Created();
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> SignOut(CancellationToken cancellationToken)
    {
        await services.Logout(HttpContext);
        return NoContent();
    }

    [HttpPatch("password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUserPassword(LoggedInUserUpdatePasswordRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateUserPassword(HttpContext, requestDto, cancellationToken);
        return NoContent();
    }


    [HttpPatch("profile")]
    [Authorize]
    [ProducesResponseType(typeof(UserDataResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUserPersonalInfo(UserUpdatePersonalDataRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateUserPersonalInfo(HttpContext, requestDto, cancellationToken);
        return Ok();
    }

    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(LoggedInUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        await services.RefreshAccessToken(HttpContext);
        return Ok();
    }
}