using Microsoft.AspNetCore.Identity;
using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Interfaces;
using MyTravel.Domain.Entities;

namespace MyTravel.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> uManager;
    private readonly IJwtTokenService TService;

    public AuthService(UserManager<User> userManager, IJwtTokenService TokenService)
    {
        uManager = userManager;
        TService = TokenService;
    }

    public async Task<(bool Succeeded, AuthResponseDto? Response, IEnumerable<string> Errors)> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await uManager.FindByEmailAsync(dto.Email);
         if(existingUser != null)
        {
            return (false, null, new[] {"El correo electrónico ya está registrado."});
        }

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result1 = await uManager.CreateAsync(user, dto.Password);
        if (!result1.Succeeded)
        {
            return(false, null, result1.Errors.Select(e => e.Description ));
        }
        var roles = await uManager.GetRolesAsync(user);
        var token = TService.GenerateToken(user,roles);

        var response = new AuthResponseDto
        {
            UserId= user.Id,
            Email = user.Email,
            FullName = user.FullName,                                                                       
            Token = token
        };
        return (true, response, Enumerable.Empty<string>());
    }

    public async Task<(bool Succeeded, AuthResponseDto? Response, string? ErrorMessage)> LoginAsync(LoginRequestDto dto)
    {
        var user = await uManager.FindByEmailAsync(dto.Email);
        if(user == null)
        {
            return (false, null, "Credenciales Invalidas");
        }
    
        var isPassawordValid = await uManager.CheckPasswordAsync(user , dto.Password);
        if (!isPassawordValid)
        {
            return (false,null, "Credenciales Invalidas.");
        }
        var roles = await uManager.GetRolesAsync(user);
        var token = TService.GenerateToken(user,roles);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Token = token
        }; 
        return (true, response, null);
    }
}
