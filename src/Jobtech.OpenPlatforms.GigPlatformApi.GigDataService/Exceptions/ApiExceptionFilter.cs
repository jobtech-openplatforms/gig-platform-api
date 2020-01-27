using System;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.Exceptions
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        ApiError apiError = null;
            context.HttpContext.Response.ContentType = "application/json";
        if (context.Exception is ApiException)
        {
            // handle explicit 'known' API errors
            var ex = context.Exception as ApiException;
            context.Exception = null;
            apiError = new ApiError(ex);
            apiError.errors = ex.Errors;

            context.HttpContext.Response.StatusCode = ex.StatusCode;
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            apiError = new ApiError("Unauthorized Access");
            context.HttpContext.Response.StatusCode = 401;
            
            // handle logging here
        }
        else
        {
            // Unhandled errors
            #if !DEBUG
                var msg = "An unhandled error occurred.";                
                string stack = null;
            #else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
            #endif

            apiError = new ApiError(context.Exception.GetBaseException());
            //apiError.detail = stack;

            context.HttpContext.Response.StatusCode = 500;

            // handle logging here
        }

        // always return a JSON result
        context.Result = new JsonResult(apiError);

        base.OnException(context);
    }
}
}