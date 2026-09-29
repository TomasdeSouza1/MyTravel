using Microsoft.AspNetCore.Mvc;
using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Interfaces;

namespace MyTravel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService serviceAuth;

    public AuthController(IAuthService authService)
    {
        serviceAuth = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var (succeeded,response, errors)= await serviceAuth.RegisterAsync(dto);
        if (!succeeded)
        {
            return BadRequest(new {errors});
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
         var (succeeded,response, ErrorM)= await serviceAuth.LoginAsync(dto);
            if (!succeeded)
            {
                return Unauthorized(new {message = ErrorM});
            }
        return Ok(response);
    }
}
