using Core.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace Core.Specification
{
    public static class AccountSpecification
    {
        public static IQueryable<IdentityUser> UsersByPage(this IQueryable<User> query, int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            int pageSize = 10;
            return query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
