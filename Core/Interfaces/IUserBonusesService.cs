using Core.DTOs.UserInfo;

namespace Core.Interfaces
{
    public interface IUserBonusesService
    {
        Task<List<UserBonusesDTO>> GetUserBonuses(string userId);
    }
}
