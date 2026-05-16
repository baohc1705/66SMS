using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Abstractions.Repositories.Sql.Base;
using _66SMS.Persistence.Commons.Mappers;
using _66SMS.Persistence.Configurations.Identity;
using _66SMS.Persistence.Repositories.Sql;
using _66SMS.Persistence.Repositories.Sql.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace _66SMS.Persistence.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
        {
           
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConn")));

            
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(IGenericSqlRepository<,>), typeof(GenericSqlRepository<,>));
            services.AddScoped<IUserSqlRepository, UserSqlRepository>();
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(typeof(ApplicationUserMapperProfile));
            });
            return services;
        }
    }
}
