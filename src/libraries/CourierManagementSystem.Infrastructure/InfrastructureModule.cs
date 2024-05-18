using Autofac;
using CourierManagementSystem.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;

namespace CourierManagementSystem.Infrastructure;

public class InfrastructureModule : Module
{
    private readonly string _connectionString;
    private readonly string _migrationAssemblyName;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InfrastructureModule(string connectionString,
                                string migrationAssemblyName,
                                IWebHostEnvironment webHostEnvironment)
    {
        _connectionString = connectionString;
        _migrationAssemblyName = migrationAssemblyName;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationDbContext>().AsSelf()
            .WithParameter("connectionString", _connectionString)
            .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            .InstancePerLifetimeScope();

        builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
            .WithParameter("connectionString", _connectionString)
            .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            .InstancePerLifetimeScope();

        base.Load(builder);
    }
}
