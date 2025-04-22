using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is FluentValidation.ValidationException fluentException)
            {
                problemDetails.Title = "one or more validation errors occurred.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationErrors = fluentException.Errors.Select(x => x.ErrorMessage).ToList();
                problemDetails.Extensions.Add("errors", validationErrors);
            }
            else
            {
                problemDetails.Title = exception.Message;
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
                problemDetails.Extensions.Add("exception", GetExceptionMessage());
            }

            logger.LogError("{ProblemDetailsTitle}", exception);

            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken)
                  .ConfigureAwait(false);
            return true;
        }

        private static string GetExceptionMessage()
        {
            var errorMessages = new List<string>()
            {
                "[APP CRASH] The application has unexpectedly quit.",
                "[ERROR] Windows could not detect your keyboard. Press F1 to continue.",
                "Kernel Panic: The system has panicked due to a critical error.",
                "Segmentation Fault: The application attempted therapy and failed",
                "Emotion Overflow: The application has run out of emotional space.",
                "Application Error: The application is taking a power nap, try again later"
            };
            var random = new Random();
            var randomIndex = random.Next(errorMessages.Count);
            return errorMessages[randomIndex];
        }
    }
}
