using Core.Entities.UserInfo;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.UserInfo
{
    public class BagDTO
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<BagItems>? BagItems { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
