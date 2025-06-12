namespace Core.DTOs.User
{
    public class ConfirmEmailDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
