using Core.Entities.UserInfo;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.UserEntity
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
        public decimal BonusBalance {get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<UserFavoriteProduct>? FavoriteProducts { get; set; }
        public ICollection<UserProductReview>? UserProductReview { get; set; }
        public ICollection<UserBonuses>? UserBonuses { get; set; }
        public Bag? Bag { get; set; }
        public int? BagId { get; set; }

        // type
        public string ClientId { get; set; } = string.Empty;
        public string AuthType {  get; set; } = string.Empty;
    }
}