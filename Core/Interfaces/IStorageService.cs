using Core.DTOs.Storage;

namespace Core.Interfaces
{
    public interface IStorageService
    {
        Task AddQuantityStorageAsync(StorageDTO [] storagesDTO);
        Task CreateStorageForProduct(StorageDTO storageDTO);
        Task<List<StorageDTO>> GetStorageByProductIdAsync(int productId);
        Task<StorageDTO> GetStorageSizeByProductIdAsync(int productId, string size);
        Task ChangeDecreaseQuantityStorageAsync(int productId, string size, int quantity);
        Task ChangeIncreaseQuantityStorageAsync(int productId, string size, int quantity);
    }
}
