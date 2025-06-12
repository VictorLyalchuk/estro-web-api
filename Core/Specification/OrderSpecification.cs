using Ardalis.Specification;
using Core.Entities.UserInfo;

namespace Core.Specification
{
    public class OrderSpecification
    {
        public class GetAllOrders : Specification<Order>
        {
            public GetAllOrders()
            {
                Query
                    .Include(p => p.OrderItems)
                    .Include(p => p.Address)
                    .Include(p => p.OrderItems)
                    .ThenInclude(p => p.OrderPayment);
            }
        }
        public class GetOrderById : Specification<Order>
        {
            public GetOrderById(string Id, int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 1;

                var query = Query.OrderByDescending(p => p.Id)
                     .Where(p => p.UserId == Id)
                     .Include(p => p.OrderItems)
                     .Include(p => p.Address)
                    .Include(p => p.OrderItems)
                    .ThenInclude(p => p.OrderPayment);

                var result = query
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
        public class GetOrderCountByIdSpecification : Specification<Order>
        {
            public GetOrderCountByIdSpecification(string Id)
            {
                Query.Where(p => p.UserId == Id);
            }
        }
    }
}