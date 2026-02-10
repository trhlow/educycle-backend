using System.Net;

namespace EduCycle.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }
}
