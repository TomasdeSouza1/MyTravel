using Microsoft.AspNetCore.Identity;
using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Interfaces.Auth;
using MyTravel.Domain.Entities;

namespace MyTravel.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(bool Succeeded, AuthResponseDto? Response, IEnumerable<string> Errors)> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return (false, null, new[] { "El correo electrónico ya está registrado." });
        }

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            LockoutEnabled = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return (false, null, result.Errors.Select(e => e.Description));
        }

        // Asignar rol "User" por defecto
        const string defaultRole = "User";
        if (!await _roleManager.RoleExistsAsync(defaultRole))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(defaultRole));
        }
        await _userManager.AddToRoleAsync(user, defaultRole);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Token = token
        };
        return (true, response, Enumerable.Empty<string>());
    }

    public async Task<(bool Succeeded, AuthResponseDto? Response, string? ErrorMessage)> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return (false, null, "Credenciales inválidas.");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return (false, null, "La cuenta está bloqueada temporalmente por demasiados intentos fallidos. Intente más tarde.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            await _userManager.AccessFailedAsync(user);
            if (await _userManager.IsLockedOutAsync(user))
            {
                return (false, null, "La cuenta está bloqueada temporalmente por demasiados intentos fallidos. Intente más tarde.");
            }
            return (false, null, "Credenciales inválidas.");
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

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
