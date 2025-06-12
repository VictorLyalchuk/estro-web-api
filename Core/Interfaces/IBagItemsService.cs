using Core.DTOs.UserInfo;
using Core.Entities.UserInfo;

namespace Core.Interfaces
{
    public interface IBagItemsService
    {
        Task CreateAsync(BagItemsDTO bagDTO);
        Task EditAsync(BagItems bagItems);
        Task DeleteBagItemsByIdBagAsync(BagItems bagItems);
        Task DeleteItemByID(int id);
        Task<List<BagItemsDTO>>? GetBagItemsByBagIdAsync(int BagId);
        Task<List<BagItemsDTO>>? GetBagItemsByBagEmailAsync(string email);
    }
}