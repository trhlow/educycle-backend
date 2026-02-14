using System.Net;

namespace EduCycle.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, (int)HttpStatusCode.Unauthorized)
    {
    }
}
