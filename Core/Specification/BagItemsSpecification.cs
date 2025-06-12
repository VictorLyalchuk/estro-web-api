using Ardalis.Specification;
using Core.DTOs.UserInfo;
using Core.Entities.UserInfo;

namespace Core.Specification
{
    public class BagItemsSpecification
    {
        public class GetBagItemByProps : Specification<BagItems>
        {
            public GetBagItemByProps(BagDTO bag)
            {
                Query
                .Where(f => f.BagId == bag.Id && f.ProductId == bag.ProductId && f.Size == bag.Size);
            }
        }
        public class GetBagItemByPropsItems : Specification<BagItems>
        {
            public GetBagItemByPropsItems(BagItems bag)
            {
                Query
                .Where(f => f.BagId == bag.Id && f.ProductId == bag.ProductId && f.Size == bag.Size);
            }
        }
        public class GetBagItemsByBagID : Specification<BagItems>
        {
            public GetBagItemsByBagID(int bagId)
            {
                Query
                .Where(f => f.BagId == bagId);
            }
        }
        public class GetBagItemsByBagEmail : Specification<BagItems>
        {
            public GetBagItemsByBagEmail(string email)
            {
                Query
                .Where(f => f.Bag.UserEmail == email)
                .Include(f => f.Product)
                .ThenInclude(f => f.Images);
            }
        }
    }
}