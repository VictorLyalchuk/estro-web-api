using Core.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class BagConfiguration : IEntityTypeConfiguration<Bag>
    {
        public void Configure(EntityTypeBuilder<Bag> builder)
        {
            //Set Primary Key
            builder.HasKey(x => x.Id);

            //Set Property configurations

            builder.HasOne(b => b.User)
                    .WithOne(u => u.Bag)
                    .HasForeignKey<Bag>(b => b.UserId);

            //Set Relationship configurations
            builder.HasMany(x => x.BagItems)
                   .WithOne(x => x.Bag)
                   .HasForeignKey(x => x.BagId);
        }
    }
}
