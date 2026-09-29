using MyTravel.Application.DTOs.Auth;

namespace MyTravel.Application.Interfaces;

public interface IAuthService
{
    Task<(bool Succeeded, AuthResponseDto? Response, IEnumerable<string> Errors)> RegisterAsync(RegisterRequestDto dto);
    Task<(bool Succeeded, AuthResponseDto? Response, string? ErrorMessage)> LoginAsync(LoginRequestDto dto);
}
