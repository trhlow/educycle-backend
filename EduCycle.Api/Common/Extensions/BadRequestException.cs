using System.Net;

namespace EduCycle.Common.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base(message, (int)HttpStatusCode.BadRequest)
    {
    }
}
