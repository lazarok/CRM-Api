using System.Net;
using CRM.Application.Models;

namespace CRM.Application.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(
        string entity, 
        string id, 
        List<string>? errors = default, 
        HttpStatusCode statusCode = HttpStatusCode.NotFound) 
        : base($"Not found {entity} with id '{id}'", errors, statusCode, Models.ResponseCode.NotFound)
    {
    }
}