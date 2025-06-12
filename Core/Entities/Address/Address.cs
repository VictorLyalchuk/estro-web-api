using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Address
{
    public class AddressEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
    }
}
