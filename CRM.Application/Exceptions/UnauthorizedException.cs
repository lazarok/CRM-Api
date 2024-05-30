using System.Net;

namespace CRM.Application.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(
        string message, 
        List<string>? errors = default, 
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized,
        string errorStatus = "Unauthorized")
        : base(message, errors, statusCode, errorStatus)
    {
    }
}