using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserInfo
{
    public class DailyOrderTotalDTO
    {
        public DateTime Date { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
