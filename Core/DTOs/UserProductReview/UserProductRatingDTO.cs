namespace Core.DTOs.UserProductReview
{
    public class UserProductRatingDTO
    {
        public int AverageRating { get; set; }
        public int TotalCount { get; set; }
        public List<UserProductCountsDTO> UserProductCounts { get; set; } 
    }
}
