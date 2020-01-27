using System.Net;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class GenericResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }

        public static GenericResponse Ok(string message)
            => new GenericResponse { Success = true, StatusCode = 200, Message = message };

        public static GenericResponse Failed(string message, HttpStatusCode httpStatusCode)
            => new GenericResponse { Success = false, StatusCode = (int)httpStatusCode, Message = message };
    }
}