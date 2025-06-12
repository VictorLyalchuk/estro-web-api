using Core.Entities.Product;

namespace Core.DTOs.Product
{
    public class ProductDTO
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
        public List<string> Highlights_en { get; set; }
        public List<string> Highlights_uk { get; set; }
        public List<string> Highlights_es { get; set; }
        public List<string> Highlights_fr { get; set; }
        public decimal Price { get; set; }
        public string Color_en { get; set; } = string.Empty;
        public string Color_uk { get; set; } = string.Empty;
        public string Color_es { get; set; } = string.Empty;
        public string Color_fr { get; set; } = string.Empty;
        public string Season_en { get; set; } = string.Empty;
        public string Season_uk { get; set; } = string.Empty;
        public string Season_es { get; set; } = string.Empty;
        public string Season_fr { get; set; } = string.Empty;
        public string Material_en { get; set; } = string.Empty;
        public string Material_uk{ get; set; } = string.Empty;
        public string Material_es { get; set; } = string.Empty;
        public string Material_fr { get; set; } = string.Empty;
        public string Article { get; set; } = string.Empty;
        public List<ImageEntity>? Images { get; set; }
        public List<string>? ImagesPath { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName_en { get; set; } = string.Empty;
        public string CategoryName_uk { get; set; } = string.Empty;
        public string CategoryName_es { get; set; } = string.Empty;
        public string CategoryName_fr { get; set; } = string.Empty;
        public string SubCategoryName_en { get; set; } = string.Empty;
        public string SubCategoryName_uk { get; set; } = string.Empty;
        public string SubCategoryName_es { get; set; } = string.Empty;
        public string SubCategoryName_fr { get; set; } = string.Empty;
        public string MainCategoryName_en { get; set; } = string.Empty;
        public string MainCategoryName_uk { get; set; } = string.Empty;
        public string MainCategoryName_es { get; set; } = string.Empty;
        public string MainCategoryName_fr { get; set; } = string.Empty;
        public List<StorageEntity>? Storages { get; set; }
        public int StorageQuantity { get; set; }
        public string URLCategoryName { get; set; } = string.Empty;
        public string URLSubCategoryName { get; set; } = string.Empty;
        public string URLMainCategoryName { get; set; } = string.Empty;
    }
}