namespace Core.DTOs.Product
{
    public class ProductSizeDTO
    {
        public int Id { get; set; }
        public string? Name_en { get; set; }
        public string? Name_es { get; set; }
        public string? Name_uk { get; set; }
        public string? Name_fr { get; set; }
        public string? Value { get; set; }
        public int? Info { get; set; }
        public int SortOrder { get; set; }
    }
}
