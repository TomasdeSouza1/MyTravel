using MyTravel.Domain.Entities;

namespace MyTravel.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user, IList<string> roles);
}
