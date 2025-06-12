using Core.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.EntitiesConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            //Set Primary Key
            builder.HasKey(c => c.Id);

            //Set Property configurations
            builder.Property(c => c.Name_en)
                   .HasMaxLength(180)
                   .IsRequired();

            builder.Property(c => c.Name_uk)
                   .HasMaxLength(180)
                   .IsRequired();

            builder.Property(c => c.Name_fr)
                   .HasMaxLength(180)
                   .IsRequired();

            builder.Property(c => c.Name_es)
                   .HasMaxLength(180)
                   .IsRequired();


            //Set Relationship configurations
            builder.HasMany(c => c.Products)
                   .WithOne(c => c.Category)
                   .HasForeignKey(c => c.CategoryId);

            // Category to SubCategory (One-to-Many)
            builder
                .HasOne(c => c.SubCategory)
                .WithMany(s => s.Categories)
                .HasForeignKey(s => s.SubCategoryId);

            // Optionally, configure cascade delete behavior if needed
            builder
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
