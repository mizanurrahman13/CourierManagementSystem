using Autofac;
using CourierManagementSystem.Web.Areas.Operator.Models;

namespace CourierManagementSystem.Web;

public class WebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BookParcelCreateModel>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BookParcelListModel>()
            .AsSelf()
            .InstancePerLifetimeScope();        

        base.Load(builder);
    }
}
