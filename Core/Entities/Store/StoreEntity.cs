using Core.Entities.Address;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Store
{
    public class StoreEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name_en { get; set; }
        public string? Name_es { get; set; }
        public string? Name_fr { get; set; }
        public string? Name_uk { get; set; }
        public string? WorkingHours { get; set; }
        public string? MapLink { get; set; }
        public string? Address_en { get; set; }
        public string? Address_es { get; set; }
        public string? Address_fr { get; set; }
        public string? Address_uk { get; set; }
        public int? CityId { get; set; }
        public CityEntity? City { get; set; }
    }
}
