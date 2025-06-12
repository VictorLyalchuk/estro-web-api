using Core.DTOs.Storage;

namespace Core.DTOs.UserFavoriteProduct
{
    public class UserFavoriteProductDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName_en { get; set; }
        public string ProductName_uk { get; set; }
        public string ProductName_es { get; set; }
        public string ProductName_fr { get; set; }
        public int ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        public List<StorageDTO>? Storages { get; set; }
    }
}
