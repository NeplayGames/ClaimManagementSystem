using System.Net;

namespace ClaimsManagementSystem.Exceptions;

public sealed class BusinessRuleException : AppException
{
    public BusinessRuleException(string message, string code = "business_rule_violation")
        : base(message, HttpStatusCode.BadRequest, code)
    {
    }
}
