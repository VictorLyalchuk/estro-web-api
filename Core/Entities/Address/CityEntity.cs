using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Address
{
    public class CityEntity
    {
        [Key]
        public int Id { get; set; }
        public string? CityName_en { get; set; }
        public string? CityName_es { get; set; }
        public string? CityName_fr { get; set; }
        public string? CityName_uk { get; set; }
        public int? CountryID { get; set; }
        public CountryEntity? Country { get; set; }
    }
}
