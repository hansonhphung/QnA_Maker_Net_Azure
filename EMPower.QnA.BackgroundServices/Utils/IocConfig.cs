using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EMPower.QnA.BackgroundServices.Jobs.Implements;
using EMPower.QnA.Data.Context;
using EMPower.QnA.Data.Implementations;
using EMPower.QnA.Data.Abstractions;

namespace EMPower.QnA.BackgroundServices.Utils
{
    public static class IocConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            //Register dbcontext
            builder.Register<QnAEntities>(c => QnAContextFactory.CreateContext()).InstancePerLifetimeScope();
            //Regiser services/component
            var serviceAsssembly = typeof(BaseService).Assembly;
            builder.RegisterAssemblyTypes(serviceAsssembly)
                .Where(t => t.Namespace.Contains("EMPower.QnA.Data.Implementations")
                || t.IsAssignableTo<IServices>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterType<PushLatestQnAJob>().AsSelf().InstancePerLifetimeScope();


            //Quartz
            builder.RegisterType<AutofacJobFactory>().As<IJobFactory>().InstancePerLifetimeScope();
            builder.Register(x => new StdSchedulerFactory().GetScheduler()).As<IScheduler>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
