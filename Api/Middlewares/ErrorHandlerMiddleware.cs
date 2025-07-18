using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); 
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest, // 400
            KeyNotFoundException => HttpStatusCode.NotFound,    // 404
            _ => HttpStatusCode.InternalServerError             // 500
        };

        response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { message = exception.Message });
        return response.WriteAsync(result);
    }
}
