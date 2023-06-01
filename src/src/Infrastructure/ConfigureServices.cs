using Application.Common.Interfaces;
using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Identity.Security;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
//using Infrastructure.Services;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

              
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.TryAddTransient<IApplicationDbContext, ApplicationDbContext>();
         
            // services.TryAddScoped<ApplicationDbContextInitialiser>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.TryAddTransient<IDateTime, DateTimeService>();
        services.TryAddTransient<IIdentityService, IdentityService>();
        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
