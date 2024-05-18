using Autofac.Extensions.DependencyInjection;
using Autofac;
using CourierManagementSystem.Infrastructure;
using CourierManagementSystem.Infrastructure.Contexts;
using CourierManagementSystem.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Configuration;
using CourierManagementSystem.Web.Areas.Operator.Models;
using CourierManagementSystem.Web;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddSingleton<AdminDataSeed>();
//var connectionStringName = builder.Configuration.GetConnectionString("DefaultConnection")!;
//var assemblyName = Assembly.GetExecutingAssembly().FullName;

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//var webHostEnvironment = builder.Environment;

//builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
//{   
//    containerBuilder.RegisterModule(new InfrastructureModule(connectionStringName!, assemblyName!, webHostEnvironment));
//});
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebModule());
});

builder.Services
    .AddInfrastructure(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

var app = builder.Build();

//var seedInstance = app.Services
//                        .CreateScope().ServiceProvider
//                        .GetRequiredService<AdminDataSeed>();

//await seedInstance.SeedUserAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Rolemanager}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
