namespace NureTimetableAPI.Controllers.Responses;

public class ResponseMessage
{
    public string Message { get; set; }

    public int StatusCode { get; set; }

    public ResponseMessage(string message, int statusCode = 200)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
