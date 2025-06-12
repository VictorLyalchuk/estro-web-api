namespace Core.DTOs.UserProductReview
{
    public class CreateUserProductReviewDTO
    {
        public int Rating { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}