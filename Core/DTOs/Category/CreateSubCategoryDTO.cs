namespace Core.DTOs.Category
{
    public class CreateSubCategoryDTO
    {
        public string Name_en { get; set; }
        public string Name_es { get; set; }
        public string Name_uk { get; set; }
        public string Name_fr { get; set; }
        public string? Description_en { get; set; }
        public string? Description_uk { get; set; }
        public string? Description_es { get; set; }
        public string? Description_fr { get; set; }
        public string URLName { get; set; }
        public string? ImagePath { get; set; }
        public int? MainCategoryId { get; set; }
    }
}
