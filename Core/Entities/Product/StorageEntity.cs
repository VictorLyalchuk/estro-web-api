using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Product
{
    public class StorageEntity
    {
        [Key]
        public int Id { get; set; }
        public int? ProductQuantity { get; set; }
        public string Size { get; set; }
        public bool inStock  { get; set; } = false;
        public int SortOrder { get; set; }
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
