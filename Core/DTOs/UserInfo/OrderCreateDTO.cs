namespace Core.DTOs.UserInfo
{
    public class OrderCreateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? EmailUser { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? Payment { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Discount { get; set; }
        public string? CardNumber { get; set; }
        public string? CardMonthExpires { get; set; }
        public string? CardYearExpires { get; set; }
        public string? CardHolderName { get; set; }
    }
}
