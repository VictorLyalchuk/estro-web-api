using Core.DTOs.Image;
using Core.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImageService
    {
        Task <ImageEntity> CreateImageAsync(IFormFile ImageFile);
        Task CreateAsync(ImageDTO imageDTO);
        Task <List<ImageDTO>>? GetImageByIDProductAsync(int id);
        Task DeleteAsync(int id);
        Task DeleteImageAsync(ImageDTO imageDTO);
        Task<ImageDTO> GetByIdAsync(int id);
        Task EditAsync(ImageDTO imageDTO);

        Task<string> CreateUserImageAsync(IFormFile ImageFile);
        Task DeleteUserImageAsync(string image);
        Task<string> SaveImageFromUrlAsync(string url);

        Task <string> CreateCategoryImageAsync(IFormFile ImageFile);
        Task DeleteCategoryImageAsync(string ImagePath);
    }
}
