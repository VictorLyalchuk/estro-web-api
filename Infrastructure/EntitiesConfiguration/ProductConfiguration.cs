using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Product;

namespace Infrastructure.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            //Set Primary Key
            builder.HasKey(x => x.Id);

            //Set Property configurations
            builder.Property(x => x.Name_en)
                   .HasMaxLength(180)
                   .IsRequired();

            builder.Property(x => x.Description_en)
                   .HasMaxLength(1024);

            //Set Relationship configurations
            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.CategoryId).IsRequired();

            //builder.HasOne(p => p.Storage)
            //       .WithOne()
            //       .HasForeignKey<Storage>(s => s.ProductId)
            //       .IsRequired(false)
            //       .OnDelete(DeleteBehavior.Cascade); ;

            builder.HasMany(p => p.Storages)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);


        }


    }
}
