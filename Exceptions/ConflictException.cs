using System.Net;

namespace ClaimsManagementSystem.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message, string code = "conflict")
        : base(message, HttpStatusCode.Conflict, code)
    {
    }
}
