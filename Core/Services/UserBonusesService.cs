using AutoMapper;
using Core.DTOs.UserInfo;
using Core.Entities.UserInfo;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class UserBonusesService : IUserBonusesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<UserBonuses> _userBonuses;
        public UserBonusesService(IMapper mapper, IRepository<UserBonuses> userBonuses)
        {
            _mapper = mapper;
            _userBonuses = userBonuses;
        }
        public async Task<List<UserBonusesDTO>> GetUserBonuses(string userId)
        {
            var bonuses = await _userBonuses.GetListBySpec(new UserBonusesSpecification.GetbyUserId(userId));
            return _mapper.Map<List<UserBonusesDTO>>(bonuses);
        }
    }
}
