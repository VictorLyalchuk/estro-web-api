using Ardalis.Specification;
using Core.Entities.UserInfo;

namespace Core.Specification
{
    public class OrderItemsSpecification
    {
        public class GetOrderItemsSpecificationByStep : Specification<OrderItems>
        {
            public GetOrderItemsSpecificationByStep(int page, int pageSize, int[] step)
            {
                if (page < 1)
                {
                    page = 1;
                }
                if(pageSize < 1)
                {
                    pageSize = 10;
                }
                Query.OrderByDescending(p => p.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Where(p => step.Contains(p.Step))
                     .Include(p => p.Product)
                     .Include(p => p.Order)
                     .Include(p => p.OrderPayment)
                     .Include(p => p.Order)
                     .ThenInclude(p => p.Address)
                     .Include(p => p.Product)
                     .ThenInclude(p => p.Storages);
            }
        }
        public class GetOrderQuantitySpecificationByStep : Specification<OrderItems>
        {
            public GetOrderQuantitySpecificationByStep(int[] step)
            {

                Query.Where(p => step.Contains(p.Step));
            }
        }
    }
}
