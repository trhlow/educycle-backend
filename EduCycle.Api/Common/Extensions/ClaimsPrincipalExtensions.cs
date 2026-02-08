using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EduCycle.Api.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.Parse(userId!);
    }
}
