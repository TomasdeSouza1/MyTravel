using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Interfaces.Auth;

namespace MyTravel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var (succeeded, response, errors) = await _authService.RegisterAsync(dto);
        if (!succeeded)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Error de registro",
                detail: string.Join("; ", errors ?? []));
        }

        return Ok(response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var (succeeded, response, errorMessage) = await _authService.LoginAsync(dto);
        if (!succeeded)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "No autorizado",
                detail: errorMessage ?? "Credenciales inválidas.");
        }
        return Ok(response);
    }
}
