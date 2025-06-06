using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exception.Message, DateTime.UtcNow);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException=>(
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode=StatusCodes.Status400BadRequest
                ),  
                BadRequestException=>(
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException=>(
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ =>(
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
            };

            var problemDetails = new ProblemDetails
            {
                Detail = details.Detail,
                Title= details.Title,
                Status=details.StatusCode,
                Instance=httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if(exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }
    }
}
