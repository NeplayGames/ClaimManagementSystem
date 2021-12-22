using System.Net;

namespace ClaimsManagementSystem.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound, "not_found")
    {
    }
}
