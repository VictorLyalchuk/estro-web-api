namespace Core.DTOs.User
{
    public class UserCreateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Role { get; set; }
        public string? AuthType { get; set; }
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
    }
}
