namespace Core.DTOs.User
{
    public class UserDTO
    {
        public string ID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public decimal BonusBalance { get; set; }
        public DateTime Birthday { get; set; }
        public string AuthType { get; set; } = string.Empty;
        public bool? IsBlocked { get; set; }
    }
}
