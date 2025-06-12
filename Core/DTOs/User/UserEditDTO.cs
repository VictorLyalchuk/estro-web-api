namespace Core.DTOs.User
{
    public class UserEditDTO
    {
        public string? ID { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
        public string? Role { get; set; }
        public string? AuthType { get; set; }
        public string? ImagePath { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public bool? IsBlocked { get; set; }
    }
}
