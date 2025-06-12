using Core.Entities.Product;

namespace Core.DTOs.Product
{
    public class ProductDTOEdit
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
        public List<string>? Highlights_en { get; set; }
        public List<string>? Highlights_uk { get; set; }
        public List<string>? Highlights_es { get; set; }
        public List<string>? Highlights_fr { get; set; }
        public decimal Price { get; set; }
        public int Season { get; set; }
        public int Color { get; set; }
        public int Material { get; set; }
        public string Article { get; set; } = string.Empty;
        public List<ImageEntity>? Images { get; set; }
        public int Category { get; set; }
        public List<ProductSizeDTO>? Sizes { get; set; }
    }
}
