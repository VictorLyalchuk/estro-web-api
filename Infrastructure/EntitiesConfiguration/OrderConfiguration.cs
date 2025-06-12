using Core.Entities.UserInfo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Set Primary Key
            builder.HasKey(x => x.Id);

            //Set Property configurations

            //builder.HasMany(b => b.Users)
            //        .WithMany(u => u.Orders);

            //Set Relationship configurations
            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId);
        }
    }
}
