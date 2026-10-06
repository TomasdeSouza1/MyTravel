using MyTravel.Domain.Entities;

namespace MyTravel.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateToken(User user, IList<string> roles);
}
