using Core.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class MainCategoryConfiguration : IEntityTypeConfiguration<MainCategory>
    {
        public void Configure(EntityTypeBuilder<MainCategory> builder)
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
            builder.HasMany(c => c.SubCategories)
                   .WithOne(c => c.MainCategory)
                   .HasForeignKey(c => c.MainCategoryId);
        }
    }
}
