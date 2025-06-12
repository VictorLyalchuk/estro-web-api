using Core.Entities.Product;

namespace Core.Entities.UserInfo
{
    public class OrderItems
    {
        public int Id { get; set; }
        public string Name_en { get; set; } = string.Empty;
        public string Name_uk { get; set; } = string.Empty;
        public string Name_es { get; set; } = string.Empty;
        public string Name_fr { get; set; } = string.Empty;
        public string Description_en { get; set; } = string.Empty;
        public string Description_uk { get; set; } = string.Empty;
        public string Description_es { get; set; } = string.Empty;
        public string Description_fr { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Step { get; set; }
        public string? Status { get; set; }
        public string? ImagePath { get; set; }
        public DateTime DueDate { get; set; }
        public string? Article { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public int? OrderPaymentId { get; set; }
        public OrderPayment? OrderPayment { get; set; }
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
