using Core.Entities.Product;
using Core.Entities.UserEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserInfo
{
    public class UserProductReview
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
