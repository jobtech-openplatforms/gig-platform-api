using System;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages
{
    public class RatingDataFetchResult
    {
        //public Guid Identifier { get;  set; }
        public decimal Value { get;  set; }
        public DateTimeOffset? Created { get; set; }
    }
}
