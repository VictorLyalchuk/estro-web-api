using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Product
{
    public class ProductMaterial
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Value { get; set; }
        public string? Name_en { get; set; }
        public string? Name_es { get; set; }
        public string? Name_uk { get; set; }
        public string? Name_fr { get; set; }
        public int? InfoId { get; set; }
        public Info? Info { get; set; }
    }
}
