using Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class ImageConfiguratio : IEntityTypeConfiguration<ImageEntity>
    {
        public void Configure(EntityTypeBuilder<ImageEntity> builder)
        {
            //Set Primary Key
            //builder.HasKey(x => x.Id);

            ////Set Property configurations

            //Set Relationship configurations
            //builder.HasOne<Product>(i => i.Product)
            //       .WithMany(p => p.Images)
            //       .HasForeignKey(i => i.ProductId);
        }
    }
}
