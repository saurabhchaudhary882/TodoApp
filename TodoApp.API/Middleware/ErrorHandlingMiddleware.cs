using System.Net;
using System.Text.Json;
using TodoApp.Core.Exceptions;

namespace TodoApp.API.Middleware
{
    /// <summary>
    /// Middleware for global error handling
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");

            var code = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred";

            // Determine status code based on exception type
            if (exception is EntityNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is TodoAppException)
            {
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            // Set response details
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            // Create error response
            var response = JsonSerializer.Serialize(new { message });
            return context.Response.WriteAsync(response);
        }
    }
}
