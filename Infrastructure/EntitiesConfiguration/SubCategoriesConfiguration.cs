using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Category;

namespace Infrastructure.EntitiesConfiguration
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
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
            builder.HasMany(c => c.Categories)
                   .WithOne(c => c.SubCategory)
                   .HasForeignKey(c => c.SubCategoryId);


            builder.HasOne(s => s.MainCategory)
                    .WithMany(m => m.SubCategories)
                    .HasForeignKey(s => s.MainCategoryId);
        }
    }
};
