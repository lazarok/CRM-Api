using CRM.Application.Helpers.Mapping;

namespace CRM.Application.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Code { get; set; }
    public string? Message { get; set; }
    
    public static ApiResponse Ok()
    {
        return Ok(ResponseCode.Ok, null);
    }
    
    public static ApiResponse Ok(string code, string? message)
    {
        return new ApiResponse()
        {
            Success = true,
            Code = code,
            Message = message
        };
    }
    
    public static ApiResponse Error(string message = "")
    {
        return Error(ResponseCode.Unhandled, message);
    }
    
    public static ApiResponse Error(string code, string message)
    {
        return new ApiResponse()
        {
            Success = false,
            Code = code,
            Message = message
        };
    }
    
    public static ApiResponse<T> OkMapped<T>(object source)
    {
        return Ok(MappingHelper.Map<T>(source));
    }
    
    public static ApiResponse<T> Ok<T>(T data)
    {
        return Ok(data, ResponseCode.Ok, null);
    }
    
    public static ApiResponse<T> Ok<T>(T data, string code, string? message)
    {
        return new ApiResponse<T>()
        {
            Success = true,
            Code = code,
            Message = message,
            Data = data
        };
    }
    
    public static ApiResponse<T> Error<T>(string code)
    {
        return Error<T>(code, code, default);
    }

    public static ApiResponse<T> Error<T>(string code, string message)
    {
        return Error<T>(code, message, default);
    }

    public static ApiResponse<T> Error<T>(string code, string message, T? data)
    {
        return new ApiResponse<T>()
        {
            Success = false,
            Code = code,
            Message = message,
            Data = data
        };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}