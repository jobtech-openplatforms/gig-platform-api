using System;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages
{
    public class ReviewDataFetchResult
    {
        public string ReviewIdentifier { get; set; }
        public DateTimeOffset? ReviewDate { get; set; }
        public string ReviewHeading { get; set; }
        public string ReviewText { get; set; }
        public string ReviewerName { get; set; }
    }
}
