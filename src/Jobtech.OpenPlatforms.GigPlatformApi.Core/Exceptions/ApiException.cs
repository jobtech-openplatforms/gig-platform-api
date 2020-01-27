using System;
using System.Collections.Generic;
using System.Net;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public ApiException(string message,
                            int statusCode = 500,
                            IEnumerable<string> errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(Exception ex, 
                            int statusCode = 500) : 
            base(ex.Message)
        {
            StatusCode = statusCode;
        }

        public ApiException(Exception ex, 
                            int statusCode = 500,
                            IEnumerable<string> errors = null) : 
            base(ex.Message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(Exception ex,
                            HttpStatusCode statusCode,
                            IEnumerable<string> errors = null) :
            base(ex.Message)
        {
            StatusCode = (int)statusCode;
            Errors = errors;
        }
    }
}