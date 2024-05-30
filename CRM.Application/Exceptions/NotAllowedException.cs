using System.Net;
using CRM.Application.Models;

namespace CRM.Application.Exceptions;

public class NotAllowedException : CustomException
{
    public NotAllowedException(
        string message, 
        List<string>? errors = default, 
        HttpStatusCode statusCode = HttpStatusCode.Forbidden, 
        string errorStatus = "Forbidden") 
        : base(message, errors, statusCode, errorStatus)
    {
    }
}