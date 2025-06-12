using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Product
{
    public class ImageEntity
    {
        [Key]
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        public int? ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
