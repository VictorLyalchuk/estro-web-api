using Core.Entities.Product;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserInfo
{
    public class BagItems
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Size { get; set; }
        public int BagId { get; set; }
        public Bag Bag { get; set; }
        public ProductEntity Product { get; set; }
    }
}
