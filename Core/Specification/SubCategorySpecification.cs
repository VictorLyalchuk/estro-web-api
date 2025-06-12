using Ardalis.Specification;
using Core.Entities.Category;

namespace Core.Specification
{
    public class SubCategorySpecification
    {
        public class All : Specification<SubCategory>
        {
            public All()
            {
                Query
                    .Include(c => c.MainCategory)
                    .Include(c => c.Categories);
            }
        }
    }
}
