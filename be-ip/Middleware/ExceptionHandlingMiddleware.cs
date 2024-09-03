using System.Net;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case ArgumentNullException:
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case UnauthorizedAccessException ex:
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            case InvalidOperationException ex:
                status = HttpStatusCode.Conflict;
                message = exception.Message;
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(new
        {
            error = message,
            status = context.Response.StatusCode
        }.ToString());
    }
}
