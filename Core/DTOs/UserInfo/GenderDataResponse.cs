using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserInfo
{
    public class GenderDataResponse
    {
        public int WomenCount { get; set; }
        public int MenCount { get; set; }
        public double WomenPercentage { get; set; }
        public double MenPercentage { get; set; }
    }
}
