using System;
using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages
{
    public class PlatformDataFetchResult
    {
      
        public int NumberOfGigs { get; set; }
        public DateTimeOffset? PeriodStart { get; set; }
        public DateTimeOffset? PeriodEnd { get; set; }
        public IList<RatingDataFetchResult> Ratings { get; set; }
        public RatingDataFetchResult AverageRating { get; set; }
        public IList<ReviewDataFetchResult> Reviews { get; set; }
    }
}
