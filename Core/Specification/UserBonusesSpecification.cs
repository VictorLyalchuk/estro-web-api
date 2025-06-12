using Ardalis.Specification;
using Core.Entities.UserInfo;

namespace Core.Specification
{
    public class UserBonusesSpecification
    {
        public class GetbyUserId : Specification<UserBonuses>
        {
            public GetbyUserId(string userId)
            {
                Query
                    .Where(f => f.UserId == userId)
                    .OrderByDescending(i => i.Id);
            }
        }
    }
}
