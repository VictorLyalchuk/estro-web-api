using Core.Entities.Product;

namespace Core.Interfaces
{
    public interface IImageForHomeService
    {
        Task<List<ImageForHome>>? GetAllImageAsync();
    }
}
