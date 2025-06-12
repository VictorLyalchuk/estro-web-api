using Ardalis.Specification;
using Core.Entities.Category;

namespace Core.Specification
{
    public class MainCategorySpecification
    {
        public class All : Specification<MainCategory>
        {
            public All()
            {
                Query
                    .Include(c => c.SubCategories.OrderBy(p => p.Id))
                    .ThenInclude(c => c.Categories.OrderBy(p => p.Id))
                    .OrderBy(c => c.Id);
            }
        }
    }
}
