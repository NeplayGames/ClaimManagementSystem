using System.Net;

namespace ClaimsManagementSystem.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message, HttpStatusCode statusCode, string code)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }
    public string Code { get; }
}
