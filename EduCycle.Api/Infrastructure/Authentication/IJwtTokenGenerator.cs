using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
