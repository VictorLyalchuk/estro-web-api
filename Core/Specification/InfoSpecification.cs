using Ardalis.Specification;
using Core.Entities.Product;

namespace Core.Specification
{
    public static class InfoSpecification
    {
        public class GetAll : Specification<Info>
        {
            public GetAll()
            {
                Query
                    .Include(f => f.Size)
                    .Include(f => f.Colors)
                    .Include(f => f.Season)
                    .Include(f => f.Material);
            }
        }
        public class GetSeason : Specification<ProductSeason>
        {
            public GetSeason()
            {
                Query
                    .OrderBy(f => f.Name_en);
            }
        }
        public class GetColors : Specification<ProductColors>
        {
            public GetColors()
            {
                Query
                    .OrderBy(f => f.Name_en);
            }
        }
        public class GetMaterials : Specification<ProductMaterial>
        {
            public GetMaterials()
            {
                Query
                    .OrderBy(f => f.Name_en);
            }
        }
        public class GetSizes : Specification<ProductSize>
        {
            public GetSizes()
            {
                Query
                    .OrderBy(f => f.Id);
            }
        }
    }
}
