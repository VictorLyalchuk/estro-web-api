namespace Core.DTOs.Store
{
    public class StoreDTO
    {
        public int Id { get; set; }
        public string Name_en { get; set; } = string.Empty;
        public string Name_es { get; set; } = string.Empty;
        public string Name_fr { get; set; } = string.Empty;
        public string Name_uk { get; set; } = string.Empty;
        public string Address_en { get; set; } = string.Empty;
        public string Address_es { get; set; } = string.Empty;
        public string Address_fr { get; set; } = string.Empty;
        public string Address_uk { get; set; } = string.Empty;
        public string WorkingHours { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int CityId { get; set; }
    }
}
