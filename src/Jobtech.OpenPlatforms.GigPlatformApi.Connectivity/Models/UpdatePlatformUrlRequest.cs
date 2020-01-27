using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;

namespace AF.GigPlatform.Connectivity.Models
{
    public class UpdatePlatformUrlRequest
    {
        public string ProjectId { get; set; }
        public string Url { get; set; }

    }
}