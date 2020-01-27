using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;

namespace AF.GigPlatform.Connectivity.Models
{
    public class UpdatePlatformStatusRequest
    {
        public string ProjectId { get; set; }
        public string Status { get; set; }

    }
}