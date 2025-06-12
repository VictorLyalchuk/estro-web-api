using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IFilesService
    {
        Task <string> SaveProductImage(IFormFile file);
        Task DeleteProductImage(string imagePath);
        Task<string> UpdateProductImage(string fileName, IFormFile file);

        Task DeleteUserImageAsync(string imagePath);
        Task<string> SaveUserImageAsync(IFormFile file);
        Task<string> SaveImageFromUrlAsync(string url);
    }
}
