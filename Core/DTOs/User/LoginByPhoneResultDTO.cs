using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.User
{
    public class LoginByPhoneResultDTO
    {
        public string token { get; set; } = string.Empty;
        public string sid { get; set; } = string.Empty;
    }

}
