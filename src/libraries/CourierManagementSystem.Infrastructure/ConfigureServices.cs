using CourierManagementSystem.Infrastructure.Contexts;
using CourierManagementSystem.Infrastructure.Profiles;
using CourierManagementSystem.Infrastructure.Repositories;
using CourierManagementSystem.Infrastructure.Services;
using CourierManagementSystem.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CourierManagementSystem.Infrastructure;

public static class ConfigureServices
{
    private const string ConnectionStringKey = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuring AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<InfrastructureProfile>();
        });

        var connectionStringName = configuration.GetConnectionString(ConnectionStringKey)!;
        var assemblyName = Assembly.GetExecutingAssembly().FullName;

        //services.AddSingleton<AdminDataSeed>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionStringName!, m => m.MigrationsAssembly(assemblyName)));

        services
            .AddIdentity<IdentityUser, IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

        services.AddControllersWithViews();
        services.AddRazorPages();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICourierManagementSystemUnitOfWork, CourierManagementSystemUnitOfWork>();
        services.AddScoped<IBookParcelRepository, BookParcelRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IBookParcelService, BookParcelService>();
        services.AddScoped<IItemService, ItemService>();

        return services;
    }
}
