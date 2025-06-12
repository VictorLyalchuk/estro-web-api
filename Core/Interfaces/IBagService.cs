using Core.DTOs.UserInfo;

namespace Core.Interfaces
{
    public interface IBagService
    {
        Task CreateAsync(BagDTO bagDTO);
        Task DeleteBagByIdAsync(string email);
        Task<BagUserDTO>? GetBagByUserEmailAsync(string email);
        Task<int> GetCountBagByEmailAsync(string email);
        Task DeleteItemByID(int id);
        Task IncreaseAsync(int id);
        Task DecreaseAsync(int id);
    }
}