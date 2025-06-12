using Core.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class FavoriteProductConfiguration : IEntityTypeConfiguration<UserFavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteProduct> builder)
        {
            //Set Primary Key
            builder.HasKey(fp => new { fp.UserId, fp.ProductId });

            //Set Property configurations

            builder.HasOne(fp => fp.User)
                   .WithMany(u => u.FavoriteProducts)
                   .HasForeignKey(fp => fp.UserId);

            //Set Relationship configurations
            builder.HasOne(fp => fp.Product)
                   .WithMany(p => p.FavoriteProducts)
                   .HasForeignKey(fp => fp.ProductId);
        }
    }
}
