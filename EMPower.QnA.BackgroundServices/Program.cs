using Autofac;
using EMPower.QnA.BackgroundServices.Abstracts;
using EMPower.QnA.BackgroundServices.Jobs;
using EMPower.QnA.BackgroundServices.Mappers;
using EMPower.QnA.BackgroundServices.Utils;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices
{
    public partial class Program : ServiceBase
    {
        public const string InstallServiceName = "EMPower.QnA.BackgroundServices";
        protected static ILog SLogger = LogManager.GetLogger(typeof(ServiceBase));
        #region Private Fields
        private readonly ScheduleJobManager _jobManager;
        #endregion Private Fields

        public Program()
        {
            InitializeComponent();

            //IOC
            var container = IocConfig.Register();

            _jobManager = new ScheduleJobManager(new AutofacJobFactory(container));
            

            //Get all concrete types that inherit for JobBase
            var types = typeof(JobBase)
                .Assembly
                .GetTypes()
                .Where(type => type != typeof(JobBase)
                    && typeof(JobBase).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && !type.IsInterface).ToList();

            //For each type, create an instance which is the scheduled job, then add that job to the Job Manager
            using (var scope = container.BeginLifetimeScope())
            {
                foreach (var job in types.Select(type => scope.Resolve(type) as JobBase)
                                        .Where(job => !job.IsExcluded))
                {
                    _jobManager.AddJob(job);
                }
            }

            AutoMapperConfig.Register();
        }

        static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            //Add an exception handler for the current domain
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            //Default debug mode to false
            var debugMode = false;
            if (args.Length > 0)
            {
                foreach (var t in args)
                {
                    switch (t.ToUpper())
                    {
                        case "/I":
                            InstallService();
                            return;
                        //Uninstall the service
                        case "/U":
                            UninstallService();
                            return;
                        //Enter the debug mode
                        case "/D":
                        default:
                            debugMode = true;
                            break;
                    }
                }
            }

            if (debugMode)
            {
                var service = new Program();
                service.OnStart(null);
                //Console.WriteLine("Service Started...");
                //Console.WriteLine("<press any key to exit...>");
                Console.Read();
            }
            else
            {
                Run(new Program());
            }
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SLogger.Error(e);
            SLogger.Error(e.ExceptionObject);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            log4net.Config.XmlConfigurator.Configure();
            ConfigurationManager.RefreshSection("appSettings");

            _jobManager.Start();
            SLogger.Info(InstallServiceName + " Service was started by " + SystemReader.GetWindowsUsername() + ".");
        }

        protected override void OnStop()
        {
            base.OnStop();

            _jobManager.Stop();
            SLogger.Info(InstallServiceName + " Service was stopped by " + SystemReader.GetWindowsUsername() + ".");
        }

        private static bool IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == InstallServiceName);
        }

        private static void InstallService()
        {
            if (IsServiceInstalled())
            {
                UninstallService();
            }
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
            }
            catch (InvalidOperationException ex)
            {
                //Console.WriteLine("INSTALLATION FAILED. Please try installing the Xoom Windows Service under the Administrator role.");
                SLogger.Error(ex);
            }
            catch (InstallException ex)
            {
                //Console.WriteLine("INSTALLATION FAILED. There may be another running service that cannot be uninstalled right now. Please try installing the Xoom Windows Service under the Administrator role.");
                SLogger.Error(ex);
            }
            catch (Exception ex)
            {
                SLogger.Error(ex);
                throw;
            }
        }

        private static void UninstallService()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
            }
            catch (InvalidOperationException ex)
            {
                //Console.WriteLine("UNINSTALLATION FAILED. Please try uninstalling the Xoom Windows Service under the Administrator role.");
                SLogger.Error(ex);
            }
            catch (InstallException ex)
            {
                //Console.WriteLine("UNINSTALLATION FAILED. The service may already be uninstalled or you do not have the right permissions. Please try uninstalling the Xoom Windows Service under the Administrator role.");
                SLogger.Error(ex);
            }
            catch (Exception ex)
            {
                SLogger.Error(ex);
                throw;
            }
        }
    }
}
