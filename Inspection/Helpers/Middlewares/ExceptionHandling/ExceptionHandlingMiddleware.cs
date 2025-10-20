using AutoMapper;
using Inspection.Application.Exceptions;
using Inspection.Application.Wrappers;
using Inspection.Domain.Entities;
using Inspection.Infrastructure.Persistence;
using System.Net;
using System.Text.Json;

namespace Inspection.Helpers.Middlewares.ExceptionHandling;


public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger,
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _context = context;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred: {Message}", e.Message);
            await LogExceptionToDatabase(context, e);
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task LogExceptionToDatabase(HttpContext httpContext, Exception exception)
    {
        try
        {
            var exceptionLog = new ExceptionLog
            {
                ExceptionType = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = GetFullStackTrace(exception),
                Source = exception.Source,
                StatusCode = httpContext.Response.StatusCode,
                Path = httpContext.Request.Path,
                QueryString = httpContext.Request.QueryString.ToString(),
                InnerExceptionType = exception.InnerException?.GetType().Name,
                InnerExceptionMessage = exception.InnerException?.Message,
                AdditionalData = GetAdditionalExceptionData(exception)
            };

            await _context.ExceptionLogs.AddAsync(exceptionLog);
            await _context.SaveChangesAsync();
        }
        catch (Exception dbEx)
        {
            // Fallback to file logging if DB logging fails
            _logger.LogError(dbEx, "Failed to log exception to database");
        }
    }

    private static string GetFullStackTrace(Exception exception)
    {
        var stackTrace = exception.StackTrace ?? string.Empty;

        var innerException = exception.InnerException;
        while (innerException != null)
        {
            stackTrace += $"\n\n--- Inner Exception ---\n{innerException.GetType().Name}: {innerException.Message}\n{innerException.StackTrace}";
            innerException = innerException.InnerException;
        }

        return stackTrace;
    }

    private static string GetAdditionalExceptionData(Exception exception)
    {
        if (exception is AutoMapperMappingException autoMapperEx)
        {
            // Use reflection to safely access properties that might not exist in all versions
            var additionalData = new Dictionary<string, string>();

            // Get TypeMap information safely
            var typeMapProperty = autoMapperEx.GetType().GetProperty("TypeMap");
            if (typeMapProperty != null)
            {
                var typeMapValue = typeMapProperty.GetValue(autoMapperEx);
                additionalData["TypeMap"] = typeMapValue?.ToString();
            }

            // Get MemberMap information safely
            var memberMapProperty = autoMapperEx.GetType().GetProperty("MemberMap");
            if (memberMapProperty != null)
            {
                var memberMapValue = memberMapProperty.GetValue(autoMapperEx);
                if (memberMapValue != null)
                {
                    var destinationNameProperty = memberMapValue.GetType().GetProperty("DestinationName");
                    if (destinationNameProperty != null)
                    {
                        additionalData["MemberMap"] = destinationNameProperty.GetValue(memberMapValue)?.ToString();
                    }
                }
            }

            return JsonSerializer.Serialize(additionalData);
        }

        return null;
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        int statusCode;
        object response;

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                response = new Response<object>
                {
                    Detail = exception.Message,
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.Unauthorized,
                    Data = null,
                    Message = "Unauthorized"
                };
                break;
            case ForbiddenAccessException:
                statusCode = StatusCodes.Status403Forbidden;
                response = new Response<object>
                {
                    Detail = exception.Message,
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.Forbidden,
                    Data = null,
                    Message = "Forbidden"
                };
                break;
            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                response = new Response<object>
                {
                    Detail = exception.Message,
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Message = "Not found"
                };
                break;
            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                response = new Response<object>
                {
                    Errors = GetErrors(exception).ToDictionary(),
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = "Validation Errors"
                };
                break;
            case AutoMapperMappingException autoMapperException:
                statusCode = StatusCodes.Status500InternalServerError;
                response = new Response<object>
                {
                    Detail = GetAutoMapperExceptionDetail(autoMapperException),
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = "Mapping error occurred"
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                response = new Response<object>
                {
                    Detail = exception.Message,
                    Succeeded = false,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = "An error occurred"
                };
                break;
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetAutoMapperExceptionDetail(AutoMapperMappingException ex)
    {
        var details = new List<string>
        {
            $"Mapping types: {ex.Types?.SourceType?.Name} -> {ex.Types?.DestinationType?.Name}",
            $"Message: {ex.Message}"
        };

        if (ex.InnerException != null)
        {
            details.Add($"Inner Exception: {ex.InnerException.Message}");
        }

        if (ex.MemberMap != null)
        {
            details.Add($"Failed Member: {ex.MemberMap.DestinationName}");
        }

        return string.Join(" | ", details);
    }

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (exception is ValidationException validationException)
        {
            errors = validationException.ErrorsDictionary;
        }

        return errors;
    }

    public static IReadOnlyDictionary<string, string[]> ParseError(string message)
    {
        var result = new Dictionary<string, string[]>();

        var parts = message.Split(',', 2);
        if (parts.Length > 0)
        {
            var main = parts[0].Split(':', 2);
            if (main.Length == 2)
                result.Add(main[0].Trim(), new[] { main[1].Trim() });
        }

        if (parts.Length > 1)
        {
            try
            {
                var innerJson = parts[1].Trim();
                var innerDict = JsonSerializer.Deserialize<Dictionary<string, string>>(innerJson);
                foreach (var kv in innerDict ?? [])
                    result.Add(kv.Key, new[] { kv.Value });
            }
            catch { /* ignore parsing error */ }
        }

        return result;
    }
}