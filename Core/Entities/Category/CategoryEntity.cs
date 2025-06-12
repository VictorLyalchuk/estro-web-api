using System.ComponentModel.DataAnnotations;
using Core.Entities.Product;


namespace Core.Entities.Category
{
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name_en { get; set; }
        public string Name_es { get; set; }
        public string Name_uk { get; set; }
        public string Name_fr { get; set; }
        public string URLName { get; set; }
        public string? Description_en { get; set; }
        public string? Description_uk { get; set; }
        public string? Description_es { get; set; }
        public string? Description_fr { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<ProductEntity> Products { get; } = new List<ProductEntity>();
        public int? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
