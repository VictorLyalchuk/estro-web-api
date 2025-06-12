using System.ComponentModel.DataAnnotations;


namespace Core.DTOs.UserInfo
{
    public class BagItemsDTO
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public int BagId { get; set; }
        public string Name_en { get; set; } 
        public string Name_uk { get; set; }
        public string Name_es { get; set; } 
        public string Name_fr { get; set; }
        public string Article { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
