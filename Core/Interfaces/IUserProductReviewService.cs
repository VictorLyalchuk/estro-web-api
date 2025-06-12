using Core.DTOs.UserProductReview;

namespace Core.Interfaces
{
    public interface IUserProductReviewService
    {
        Task AddUserProductReview(CreateUserProductReviewDTO createUserProductReviewDTO);
        Task<List<UserProductReviewDTO>> GetUserProductReview(int productId, int page);
        Task<int> GetQuantityUserProductReviewAsync(int productId);
        Task<UserProductRatingDTO> GetUserProductRating(int productId);
        Task<List<UserProductReviewDTO>> ReviewsByPageAsync(int page, int pageSize);
        Task <int> ReviewQuantityAsync();
        Task DeleteReviewByIDAsync(int id);
    }
}