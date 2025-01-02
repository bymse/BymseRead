using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.Errors;

public class ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problemDetails = new ProblemDetails();

        if (exception is ValidationException validationException)
        {
            problemDetails.Title = "Validation Error";
            problemDetails.Status = 400;
            problemDetails.Detail = validationException.ValidationResult.ErrorMessage;
            httpContext.Response.StatusCode = 400;
        }
        else
        {
            problemDetails.Title = "Internal Server Error";
            problemDetails.Status = 500;
            problemDetails.Detail = exception.Message;
            httpContext.Response.StatusCode = 500;
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext, ProblemDetails = problemDetails, Exception = exception
        });
    }
}