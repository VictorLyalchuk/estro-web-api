using Core.Entities.Category;
using Core.Entities.UserEntity;
using Core.Entities.UserInfo;
using Core.Entities.Product;
using Core.Entities.Store;
using Infrastructure.EntitiesConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Address;

namespace Infrastructure.Data
{
    public class DataBaseContext : IdentityDbContext
    {
        public DataBaseContext() : base() { }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new MainCategoryConfiguration());
            builder.ApplyConfiguration(new SubCategoryConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new OptionsConfiguration());
            builder.ApplyConfiguration(new BagConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new FavoriteProductConfiguration());

            builder.Entity<MainCategory>().HasData(SeedData.SeedMainCategory());
            builder.Entity<SubCategory>().HasData(SeedData.SeedSubCategory());
            builder.Entity<CategoryEntity>().HasData(SeedData.SeedCategory());

            builder.Entity<Info>().HasData(SeedData.SeedInfos());
            builder.Entity<ProductSize>().HasData(SeedData.SeedSizes());
            builder.Entity<ProductColors>().HasData(SeedData.SeedColors());
            builder.Entity<ProductMaterial>().HasData(SeedData.SeedMaterials());
            builder.Entity<ProductSeason>().HasData(SeedData.SeedSeason());
            builder.Entity<ProductEntity>().HasData(SeedData.SeedProduct());
            builder.Entity<ImageEntity>().HasData(SeedData.SeedImage());
            builder.Entity<StorageEntity>().HasData(SeedData.SeedStorage());
            builder.Entity<ImageForHome>().HasData(SeedData.SeedImageForHome());

            builder.Entity<CountryEntity>().HasData(SeedData.SeedCountry());
            builder.Entity<CityEntity>().HasData(SeedData.SeedCity());
            builder.Entity<StoreEntity>().HasData(SeedData.SeedStores());
        }
        public DbSet<User> User { get; set; }

        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductColors> ProductColor { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StorageEntity> Storage { get; set; }
        public DbSet<ImageForHome> ImageForHome { get; set; }


        public DbSet<CountryEntity> Country { get; set; }
        public DbSet<CityEntity> City { get; set; }
        public DbSet<StoreEntity> Store { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<UserFavoriteProduct> UserFavoriteProduct { get; set; }
        public DbSet<UserBonuses> UserBonuses { get; set; }
    }
}
