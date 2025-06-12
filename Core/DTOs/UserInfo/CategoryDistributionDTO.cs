using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserInfo
{
    public class CategoryDistributionDTO
    {
        public string Name_en { get; set; }
        public string Name_fr { get; set; }
        public string Name_es { get; set; }
        public string Name_uk { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }


    }
}
