namespace Core.DTOs.UserProductReview
{
    public class UserProductReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Avatar { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
