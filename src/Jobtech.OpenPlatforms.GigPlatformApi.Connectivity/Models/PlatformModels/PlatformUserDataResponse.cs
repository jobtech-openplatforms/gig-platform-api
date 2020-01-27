namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels
{
    public class PlatformUserDataResponse
    {
        private PlatformUserDataResponse(string requestId, string message, bool success)
        {
            RequestId = requestId;
            Message = message;
            Success = success;
        }
        public string RequestId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }


        public static PlatformUserDataResponse Response(string requestId, string message, bool success)
            => new PlatformUserDataResponse(requestId, message, success);

        public static PlatformUserDataResponse Ok(string requestId, string message) 
            => new PlatformUserDataResponse(requestId, message, true);

        public static PlatformUserDataResponse Fail(string requestId, GenericResponse response)
            => new PlatformUserDataResponse(requestId, response.Message, false);
        public static PlatformUserDataResponse Fail(string requestId, string message)
            => new PlatformUserDataResponse(requestId, message, false);
    }
}