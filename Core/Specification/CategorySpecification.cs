using Ardalis.Specification;
using Core.Entities.Category;

namespace Core.Specification
{
    public static class CategorySpecification
    {
        public class GetAllAsync : Specification<CategoryEntity>
        {
            public GetAllAsync()
            {
                Query
                    .Include(c => c.SubCategory)
                    .ThenInclude(c => c.MainCategory);
            }
        }
        public class GetAllAsyncByID : Specification<CategoryEntity>
        {
            public GetAllAsyncByID()
            {
                Query
                    .Include(c => c.Products)
                    .OrderBy(c => c.Id);
            }
        }
        public class ByID : Specification<CategoryEntity>
        {
            public ByID(int ID)
            {
                Query
                    .Include(c => c.Products)
                    .Where(c => c.Id == ID);
            }
        }
        public class GetBySubNameAsync : Specification<CategoryEntity>
        {
            public GetBySubNameAsync(string subName)
            {
                Query
                    .Where(c => c.SubCategory.URLName == subName)
                    .OrderBy(p => p.Id);
            }
        }
        public class MainCategoryByPageAsync : Specification<MainCategory>
        {
            public MainCategoryByPageAsync(int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;
                Query.OrderBy(p => p.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
        public class SubCategoryByPageAsync : Specification<SubCategory>
        {
            public SubCategoryByPageAsync(int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;
                Query.OrderBy(p => p.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
        public class CategoryByPageAsync : Specification<CategoryEntity>
        {
            public CategoryByPageAsync(int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;
                Query.OrderBy(p => p.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
    }
}
