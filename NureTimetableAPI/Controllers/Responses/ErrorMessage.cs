using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace NureTimetableAPI.Controllers.Responses;

public class ErrorMessage
{
    public string Message { get; set; }

    public int StatusCode { get; set; }

    public ErrorMessage(string message, int statusCode = 400)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
