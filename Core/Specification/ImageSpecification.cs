using Ardalis.Specification;
using Core.Entities.Product;

namespace Core.Specification
{
    public class ImageSpecification
    {
        public class ImageByProductID : Specification<ImageEntity>
        {
            public ImageByProductID(int id)
            {
                Query
                    .Where(p => p.ProductId == id);
            }
        }
    }
}
