namespace Core.DTOs.UserInfo
{
    public class OrderPaymentDTO
    {
        public string? Payment { get; set; }
        public string? PaymentMethod { get; set; }
        public string? CardNumber { get; set; }
        public string? CardMonthExpires { get; set; }
        public string? CardYearExpires { get; set; }
        public string? CardHolderName { get; set; }
    }
}
