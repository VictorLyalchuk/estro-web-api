using Core.Entities.UserEntity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Infrastructure
{
    public static class ServiceExtension
    {
        public static void AddDBContext(this IServiceCollection service, string connection)
        {
            service.AddDbContext<DataBaseContext>(options =>
            {
                //options.UseSqlServer(connection);
                options.UseNpgsql(connection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
        public static void AddRepository(this IServiceCollection service)
        {
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
        public static void AddInfrastuctureService(this IServiceCollection service)
        {
            service.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 7;
            })
                .AddEntityFrameworkStores<DataBaseContext>()
            .AddDefaultTokenProviders();
        }
    }
}
