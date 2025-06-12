using AutoMapper;
using Core.DTOs.UserProductReview;
using Core.Entities.UserEntity;
using Core.Entities.UserInfo;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class UserProductReviewService : IUserProductReviewService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<UserProductReview> _productsReviewService;
        private readonly UserManager<User> _userManager;

        public UserProductReviewService(IMapper mapper, IRepository<UserProductReview> productsReviewService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _productsReviewService = productsReviewService;
            _userManager = userManager;
        }
        public async Task AddUserProductReview(CreateUserProductReviewDTO createUserProductReviewDTO)
        {
            var user = await _userManager.FindByIdAsync(createUserProductReviewDTO.UserId);
            if (user != null)
            {
                var review = _mapper.Map<UserProductReview>(createUserProductReviewDTO);
                if (user?.ImagePath != null)
                {
                    review.Avatar = user.ImagePath;
                }
                review.Author = user.FirstName + " " + user.LastName;
                review.OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                await _productsReviewService.InsertAsync(review);
                await _productsReviewService.SaveAsync();
            }
        }

        public async Task<UserProductRatingDTO> GetUserProductRating(int productId)
        {
            var ratings = await _productsReviewService.GetListBySpec(new UserProductReviewSpecification.GetUserProductRating(productId));

            var totalRatingSum = ratings.Sum(r => r.Rating);
            var averageRating = (int)Math.Round((double)totalRatingSum / ratings.Count());
            var userProductRatingDTO = new UserProductRatingDTO
            {
                AverageRating = averageRating,
                TotalCount = ratings.Count(),
                UserProductCounts = new List<UserProductCountsDTO>()
            };
            if (ratings.Count() > 0)
            {
                var ratingCounts = new int[5];

                foreach (var rating in ratings)
                {
                    if (rating.Rating >= 1 && rating.Rating <= 5)
                    {
                        ratingCounts[rating.Rating - 1]++;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    userProductRatingDTO.UserProductCounts.Add(new UserProductCountsDTO
                    {
                        Rating = i + 1,
                        Count = ratingCounts[i]
                    });
                }

            }
            return userProductRatingDTO;
        }
        public async Task<int> GetQuantityUserProductReviewAsync(int productId)
        {
            var reviews = await _productsReviewService.GetListBySpec(new UserProductReviewSpecification.GetQuantityUserProductReview(productId));
            return reviews.Count();
        }
        public async Task<List<UserProductReviewDTO>> GetUserProductReview(int productId, int page)
        {
            var reviews = await _productsReviewService.GetListBySpec(new UserProductReviewSpecification.GetUserProductReview(productId, page));
            return _mapper.Map<List<UserProductReviewDTO>>(reviews);
        }

        public async Task<List<UserProductReviewDTO>> ReviewsByPageAsync(int page, int pageSize)
        {
            var reviews = await _productsReviewService.GetListBySpec(new UserProductReviewSpecification.GetReviewsByPage(page, pageSize));
            return _mapper.Map<List<UserProductReviewDTO>>(reviews);
        }

        public async Task <int> ReviewQuantityAsync()
        {
            var reviews = await _productsReviewService.GetAsync();
            return reviews.Count();
        }

        public async Task DeleteReviewByIDAsync(int id)
        {
            var productReviewToDelete = await _productsReviewService.GetByIDAsync(id);
            if (productReviewToDelete != null)
            {
                await _productsReviewService.DeleteAsync(productReviewToDelete);
                await _productsReviewService.SaveAsync();
            }
        }
    }
}
