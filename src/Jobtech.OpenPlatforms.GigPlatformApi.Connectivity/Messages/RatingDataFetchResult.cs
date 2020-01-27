using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Messages
{
    public class RatingDataFetchResult
    {
        //public Guid Identifier { get;  set; }
        public decimal Value { get;  set; }
        public DateTimeOffset? Created { get; set; }
    }
}
