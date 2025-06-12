using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.User
{
    public class UserLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } 
        public string AuthType { get; set; } = string.Empty;
    }
}
