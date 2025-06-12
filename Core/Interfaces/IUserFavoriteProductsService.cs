using Core.DTOs.UserFavoriteProduct;

namespace Core.Interfaces
{
    public interface IUserFavoriteProductsService
    {
        Task AddUserFavoriteProduct(string userId, int productId);
        Task DeleteUserFavoriteProduct(string userId, int productId);
        Task <List<UserFavoriteProductDTO>> GetUserFavoriteProducts(string userId);
    }
}
