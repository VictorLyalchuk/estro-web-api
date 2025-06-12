using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Product;

namespace Infrastructure.EntitiesConfiguration
{
    public class OptionsConfiguration : IEntityTypeConfiguration<ProductColors>
    {
        public void Configure(EntityTypeBuilder<ProductColors> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder
                .HasOne(o => o.Info)
                .WithMany(i => i.Colors)
                .HasForeignKey(o => o.InfoId);
        }
    }
}
