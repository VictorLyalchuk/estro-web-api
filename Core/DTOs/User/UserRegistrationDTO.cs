using Microsoft.AspNetCore.Http;

namespace Core.DTOs.User
{
    public class UserRegistrationDTO
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AuthType { get; set; }
        public string? Role { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public string? ClientId { get; set; }
    }
}
