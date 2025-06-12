namespace Core.DTOs.Storage
{
    public class StorageDTO
    {
        public int Id { get; set; }
        public int? ProductQuantity { get; set; }
        public string Size { get; set; }
        public bool inStock { get; set; }
        public int SortOrder { get; set; }
        public int ProductId { get; set; }
    }
}