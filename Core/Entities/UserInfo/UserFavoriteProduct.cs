using Core.Entities.Product;
using Core.Entities.UserEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.UserInfo
{
    public class UserFavoriteProduct
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; } 
    }
}
