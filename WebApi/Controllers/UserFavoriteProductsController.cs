using Core.DTOs.UserFavoriteProduct;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteProductsController : Controller
    {
        private readonly IUserFavoriteProductsService _favoriteProducts;
        public UserFavoriteProductsController(IUserFavoriteProductsService favoriteProducts)
        {
            _favoriteProducts = favoriteProducts;
        }
        
        [HttpPost("addFavorite")]
        public async Task<IActionResult> AddUserFavoriteProduct(CreateUserFavoriteProductDTO createFavoriteProductDTO)
        {
            await _favoriteProducts.AddUserFavoriteProduct(createFavoriteProductDTO.UserId, createFavoriteProductDTO.ProductId);
            return Ok();
        }

        [HttpDelete("deleteFavorite")]
        public async Task<IActionResult> RemoveUserFavoriteProduct(RemoveUserFavoriteProductDTO removeFavoriteProductDTO)
        {
            await _favoriteProducts.DeleteUserFavoriteProduct(removeFavoriteProductDTO.UserId, removeFavoriteProductDTO.ProductId);
            return Ok();
        }

        [HttpGet("getFavorite")]
        public async Task<IActionResult> GetUserFavoriteProducts(string userId)
        {
            var favorite = await _favoriteProducts.GetUserFavoriteProducts(userId);
            if(favorite == null)
            {
                return Ok();
            }
            return Ok(favorite);
        }
    }
}
