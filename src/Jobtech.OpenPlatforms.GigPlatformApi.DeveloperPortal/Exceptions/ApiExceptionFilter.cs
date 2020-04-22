using System;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Exceptions
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;
            context.HttpContext.Response.ContentType = "application/json";
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);
                apiError.errors = ex.Errors;

                context.HttpContext.Response.StatusCode = ex.StatusCode;
                //System.Diagnostics.Trace.TraceError("There is not always butter in the fridge. {ex}", ex);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;

                _logger.LogError("There is not always cheese in the fridge. {error}", apiError);
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

                apiError = new ApiError(msg);
                apiError.detail = stack;

                context.HttpContext.Response.StatusCode = 500;

                _logger.LogError("An error occurred. {@error}", apiError);
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}