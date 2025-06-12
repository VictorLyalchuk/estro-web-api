using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Product
{
    public class ImageForHome
    {
        [Key]
        public int Id { get; set; }
        public string? ImagePath { get; set; }
    }
}
