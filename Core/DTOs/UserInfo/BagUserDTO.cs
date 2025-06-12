using Core.Entities.UserInfo;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.UserInfo
{
    public class BagUserDTO
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<BagItems>? BagItems { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
