using Ardalis.Specification;
using Core.Entities.Store;

namespace Core.Specification
{
    public class StoreSpecification
    {
        public class StoreByPage : Specification<StoreEntity>
        {
            public StoreByPage(int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;
                Query.OrderBy(p => p.City.Country.CountryName_en) 
                     .ThenBy(p => p.City.CityName_en)            
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Include(p => p.City)
                     .ThenInclude(p => p.Country);
            }
        }
        public class StoreAll : Specification<StoreEntity>
        {
            public StoreAll()
            {
                Query.OrderBy(p => p.Id)
                    .Include(p => p.City)
                    .ThenInclude(p => p.Country);
            }
        }
    }
}
