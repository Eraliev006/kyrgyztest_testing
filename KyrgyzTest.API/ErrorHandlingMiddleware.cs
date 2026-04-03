using KyrgyzTest.Core.Exceptions;

namespace KyrgyzTest.API;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteError(context, 404, ex.Message);
        }
        catch (ValidationException ex)
        {
            await WriteError(context, 400, ex.Message);
        }
        catch (BusinessException ex)
        {
            await WriteError(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteError(context, 500, "Internal server error");
        }
    }
    
    private async Task WriteError(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new
        {
            statusCode,
            message
        });
    }
}