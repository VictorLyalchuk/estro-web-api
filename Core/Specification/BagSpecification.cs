using Ardalis.Specification;
using Core.Entities.UserInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class BagSpecification
    {
        public class GetBagByUserEmail : Specification<Bag>
        {
            public GetBagByUserEmail(string userEmail)
            {
                Query
                    .Include(f => f.BagItems)
                    //.ThenInclude(f => f.Product)
                    .Where(f => f.UserEmail == userEmail);

            }
        }
        public class GetBagByItemsID : Specification<Bag>
        {
            public GetBagByItemsID(int bagId)
            {
                Query
                    .Include(b => b.BagItems)
                    .Where(b => b.Id == bagId);
            }
        }
    }
}