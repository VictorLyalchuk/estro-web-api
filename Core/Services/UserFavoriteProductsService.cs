using AutoMapper;
using Core.Entities.UserInfo;
using Core.Entities.Product;
using Core.Entities.UserEntity;
using Core.Helpers;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Core.DTOs.UserFavoriteProduct;

namespace Core.Services
{
    public class UserFavoriteProductsService : IUserFavoriteProductsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<UserFavoriteProduct> _favoriteProductsService;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly UserManager<User> _userManager;
        public UserFavoriteProductsService(IMapper mapper, IRepository<UserFavoriteProduct> favoriteProductsService, UserManager<User> userManager, IRepository<ProductEntity> productRepository)
        {
            _mapper = mapper;
            _favoriteProductsService = favoriteProductsService;
            _userManager = userManager;
            _productRepository = productRepository;
        }
        public async Task AddUserFavoriteProduct(string userId, int productId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.UserNotFoundByEmail, HttpStatusCode.NotFound);
            }

            var product = await _productRepository.GetByIDAsync(productId);
            if (product == null)
            {
                throw new CustomHttpException(ErrorMessages.ProductNotFound, HttpStatusCode.NotFound);
            }

            var favorite = new UserFavoriteProduct
            {
                UserId = userId,
                ProductId = productId
            };
            await _favoriteProductsService.InsertAsync(favorite);
            await _favoriteProductsService.SaveAsync();
        }

        public async Task DeleteUserFavoriteProduct(string userId, int productId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new CustomHttpException(ErrorMessages.UserNotFoundByEmail, HttpStatusCode.NotFound);
            }
            
            var favorite = await _favoriteProductsService.GetItemBySpec(new UserFavoriteProductsSpecification.GetbyId(userId, productId));
            if (favorite == null)
            {
                throw new CustomHttpException(ErrorMessages.ProductNotFound, HttpStatusCode.NotFound);
            }
            await _favoriteProductsService.DeleteAsync(favorite);
            await _favoriteProductsService.SaveAsync();
        }

        public async Task<List<UserFavoriteProductDTO>> GetUserFavoriteProducts(string userId)
        {
            var favorite = await _favoriteProductsService.GetListBySpec(new UserFavoriteProductsSpecification.GetbyUserId(userId));
            return _mapper.Map<List<UserFavoriteProductDTO>>(favorite);
        }
    }
}
