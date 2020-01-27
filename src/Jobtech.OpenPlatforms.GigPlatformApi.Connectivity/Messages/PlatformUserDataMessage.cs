using System;
using System.Collections.Generic;
using System.Linq;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages
{
 public class PlatformUserDataMessage 
    {
        public PlatformUserDataMessage(string id, string username, DateTimeOffset updated,
            PlatformData platformData = null)
        {
            Id = id;
            UserName = username;
            Updated = updated;
            PlatformData = platformData;
        }

        public string Id { get; private set; }
        public string UserName { get; private set; }
        public DateTimeOffset Updated { get; private set; }
        public PlatformData PlatformData { get; private set; }
    }

    public class PlatformData
    {
        public PlatformData(int numberOfGigs, DateTimeOffset? periodStart, DateTimeOffset? periodEnd, IEnumerable<PlatformRating> ratings, IEnumerable<PlatformReview> reviews)
            : this(numberOfGigs, periodStart, periodEnd, new PlatformRatingAggregate(ratings), reviews?.ToArray()) { }

        public PlatformData(int numberOfGigs, DateTimeOffset? periodStart, DateTimeOffset? periodEnd, PlatformRatingAggregate ratings, IEnumerable<PlatformReview> reviews)
            : this(numberOfGigs, periodStart, periodEnd, ratings, reviews?.ToArray()) { }

        public PlatformData(int numberOfGigs, DateTimeOffset? periodStart, DateTimeOffset? periodEnd, PlatformRatingAggregate ratings, PlatformReview[] reviews)
        {
            NumberOfGigs = numberOfGigs;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            Ratings = ratings;
            Reviews = reviews ?? new PlatformReview[0];
        }

        public int NumberOfGigs { get; private set; }
        public DateTimeOffset? PeriodStart { get; private set; }
        public DateTimeOffset? PeriodEnd { get; private set; }
        public PlatformReview[] Reviews { get; private set; }
        public PlatformRatingAggregate Ratings { get; private set; }
    }

    public class PlatformReview
    {
        public PlatformReview(string reviewText, string reviewerName, string reviewHeading, DateTimeOffset? reviewDate)
        {
            ReviewText = reviewText;
            ReviewerName = reviewerName;
            ReviewHeading = reviewHeading;
            ReviewDate = reviewDate;
        }

        public DateTimeOffset? ReviewDate { get; private set; }
        public string ReviewHeading { get; private set; }
        public string ReviewText { get; private set; }
        public string ReviewerName { get; private set; }
    }

    public class PlatformRatingAggregate
    {
        public PlatformRatingAggregate(IEnumerable<PlatformRating> ratings)
         : this(ratings?.ToArray()) { }

        public PlatformRatingAggregate(PlatformRating[] ratings)
        {
            Ratings = ratings ?? new PlatformRating[0];
            AverageRating = ratings?.Average(r => r.Rating) ?? 0;
        }
        public decimal AverageRating { get; private set; }
        public PlatformRating[] Ratings { get; set; }
    }

    public class PlatformRating
    {
        public PlatformRating(decimal rating, DateTimeOffset? created)
        {
            Rating = rating;
            Created = created;
        }

        public DateTimeOffset? Created { get; private set; }
        public decimal Rating { get; private set; }
    }
}
