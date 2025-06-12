using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Address
{
    public class CountryEntity
    {
        [Key]
        public int Id { get; set; }
        public string? CountryName_en { get; set; }
        public string? CountryName_es { get; set; }
        public string? CountryName_fr { get; set; }
        public string? CountryName_uk { get; set; }
    }
}
