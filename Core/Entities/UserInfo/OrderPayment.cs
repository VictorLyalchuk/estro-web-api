using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserInfo
{
    public class OrderPayment
    {
        [Key]
        public int Id { get; set; }
        public string? Payment { get; set; }
        public string? PaymentMethod { get; set; }
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        public string? CardNumber { get; set; }
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiration date must be in MM/YY format.")]
        public string? CardMonthExpires { get; set; }
        public string? CardYearExpires { get; set; }
        public string? CardHolderName { get; set; }
    }
}