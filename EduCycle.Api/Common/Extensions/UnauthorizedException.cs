using System.Net;

namespace EduCycle.Common.Exceptions;

public class NotFoundexception : AppException
{
    public NotFoundexception(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }
}
