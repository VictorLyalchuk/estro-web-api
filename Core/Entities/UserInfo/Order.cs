using Core.Entities.Address;
using Core.Entities.UserEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserInfo
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? EmailUser { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OrderTotal { get; set; }
        public int? AddressId { get; set; }
        public AddressEntity? Address { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public List<OrderItems>? OrderItems { get; set; }
    }
}
