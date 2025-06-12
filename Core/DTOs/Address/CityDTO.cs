namespace Core.DTOs.Address
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string? CityName_en { get; set; }
        public string? CityName_es { get; set; }
        public string? CityName_fr { get; set; }
        public string? CityName_uk { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName_en { get; set; }
        public string? CountryName_es { get; set; }
        public string? CountryName_fr { get; set; }
        public string? CountryName_uk { get; set; }
    }
}
