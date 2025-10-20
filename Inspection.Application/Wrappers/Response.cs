using System.Net;
namespace Inspection.Application.Wrappers;

public class Response<T>
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public IDictionary<string, string[]>? Errors { get; set; }
    public T? Data { get; set; }

    public Response()
    {
    }
    public Response(T data, bool succeeded, string message = "")
    {
        Succeeded = succeeded;
        Message = message;
        Data = data;
    }
    public Response(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public Response(HttpStatusCode httpStatusCode, string message)
    {
        Succeeded = (int)httpStatusCode >= 200 && (int)httpStatusCode < 300;
        Message = message;
        HttpStatusCode = httpStatusCode;
    }

    public Response(HttpStatusCode httpStatusCode, T data, string message = "", List<string> errors = default!, bool succeeded = true)
    {
        Succeeded = succeeded;
        Message = message;
        Data = data;
        HttpStatusCode = httpStatusCode;
    }
}
