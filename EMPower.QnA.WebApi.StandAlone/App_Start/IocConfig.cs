using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using EMPower.QnA.Data.Abstractions;
using EMPower.QnA.Data.Context;
using EMPower.QnA.Data.Implementations;
using EMPower.QnA.WebApi.StandAlone.Controllers;

namespace EMPower.QnA.WebApi.StandAlone.App_Start
{
    public static class IocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            //Register dbcontext
            builder.Register<QnAEntities>(c => QnAContextFactory.CreateContext()).InstancePerLifetimeScope();

            //var webApiControllerAssembly = typeof(BaseController).Assembly;
            //builder.RegisterApiControllers(webApiControllerAssembly);

            //builder.RegisterWebApiFilterProvider(config);

            ////Register dbcontext
            //builder.Register<XchangingEntities>(c => XchangingContextFactory.CreateContext());
            //builder.RegisterType<NonePilotStorageRepository>().As<IPilotStorageRepository>().InstancePerRequest();

            var webApiControllerAssembly = typeof(QnAController).Assembly;
            builder.RegisterApiControllers(webApiControllerAssembly);

            //Regiser services/component
            var serviceAsssembly = typeof(BaseService).Assembly;
            builder.RegisterAssemblyTypes(serviceAsssembly)
                .Where(t => t.Namespace.Contains("EMPower.QnA.Data.Implementations")
                || t.IsAssignableTo<IServices>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}