using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserInfo
{
    public class PopularProductDTO
    {
        public string ProductName_en { get; set; }
        public string ProductName_fr { get; set; }
        public string ProductName_es { get; set; }
        public string ProductName_uk { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}
