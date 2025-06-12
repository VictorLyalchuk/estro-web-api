namespace Core.DTOs.Product
{
    public class InfoDTO
    {
        public int Id { get; set; }
        public string Name_en { get; set; }
        public string Name_es { get; set; }
        public string Name_fr { get; set; }
        public string Name_uk { get; set; }
        public string Value { get; set; }
        public IEnumerable<ProductColorsDTO> Colors { get; set; }
        public IEnumerable<ProductSeasonDTO> Season { get; set; }
        public IEnumerable<ProductMaterialDTO> Materials { get; set; }
        public IEnumerable<ProductSizeDTO> Sizes { get; set; }
    }
}
