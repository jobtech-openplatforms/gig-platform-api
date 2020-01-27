using System;
using System.Collections.Generic;

namespace AF.GigPlatform.Core.Entities.Api
{
    public class PlatformInteraction
    {
        public DateTime Timestamp { get; set; }
        public string InteractionId { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ServiceType { get; set; }
        public string Location { get; set; }
        public string TextContent { get; set; }

        public IEnumerable<Image> Images { get; set; } = new List<Image>();
        public double NumberOfHours { get; set; }
        public Money Income { get; set; }
        public InteractionRatings Ratings { get; set; }

        public class Image
        {
            public string Url { get; set; }
            public string Caption { get; set; }
        }

        public class Money
        {
            public static Money From(decimal amount, string currency)
                => new Money { Amount = amount, Currency = currency };

            public decimal Amount { get; set; }
            public string Currency { get; set; }
        }

        public class InteractionRatings
        {
            public IEnumerable<Rating> DetailedRatings { get; set; } = new List<Rating>();
        }

        public class Rating
        {
            public double Min { get; set; }
            public double Max { get; set; }
            public double Success { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }
    }
}