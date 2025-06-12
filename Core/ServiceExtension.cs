using Core.Interfaces;
using Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceExtension
    {
        public static void AddCustomService(this IServiceCollection service)
        {
            service.AddScoped<IAccountService, AccountService>(); 
            service.AddScoped<IBagItemsService, BagItemsService>();
            service.AddScoped<IBagService, BagService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IFilesService, FilesService>();
            service.AddScoped<IImageForHomeService, ImageForHomeService>();
            service.AddScoped<IImageService, ImageService>();
            service.AddScoped<IInfoService, InfoService>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IStorageService, StorageService>();
            service.AddScoped<IAddressService, AddressService>();
            service.AddScoped<IStoreService, StoreService>();
            service.AddScoped<IUserFavoriteProductsService, UserFavoriteProductsService>();
            service.AddScoped<IUserProductReviewService, UserProductReviewService>();
            service.AddScoped<IUserBonusesService, UserBonusesService>();
            service.AddTransient<EmailService>();

        }
        public static void AddValidator(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();

            service.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        }
        public static void AddAutoMapper(this IServiceCollection service)
        {
            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
