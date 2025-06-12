using Core.Entities.UserInfo;
using Core.Entities.Category;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Product
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name_en { get; set; } = string.Empty;
        public string Name_uk { get; set; } = string.Empty;
        public string Name_es { get; set; } = string.Empty;
        public string Name_fr { get; set; } = string.Empty;
        public string Article { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description_en { get; set; } = string.Empty;
        public string Description_uk { get; set; } = string.Empty;
        public string Description_es { get; set; } = string.Empty;
        public string Description_fr { get; set; } = string.Empty;
        public List<string>? Highlights_en { get; set; }
        public List<string>? Highlights_uk { get; set; }
        public List<string>? Highlights_es { get; set; }
        public List<string>? Highlights_fr { get; set; }
        public int SeasonId { get; set; }
        public ProductSeason Season { get; set; }
        public int ColorId { get; set; }
        public ProductColors Color { get; set; }
        public int MaterialId { get; set; }
        public ProductMaterial Material { get; set; }
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public List<ImageEntity> Images { get; set; }
        public List<StorageEntity> Storages { get; set; }
        public List<BagItems> BagItems { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public ICollection<UserFavoriteProduct> FavoriteProducts { get; set; }
        public ICollection<UserProductReview> UserProductReview { get; set; }
    }
}