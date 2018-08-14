using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace EMPower.QnA.BackgroundServices
{
    [RunInstaller(true)]
    public class CustomServiceInstaller : Installer
    {
        public CustomServiceInstaller()
        {
            var process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Username = null,
                Password = null
            };


            var service = new ServiceInstaller
            {
                ServiceName = Program.InstallServiceName,
                DisplayName = Program.InstallServiceName,
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(process);
            Installers.Add(service);

            AfterInstall += CustomServiceInstaller_AfterInstall;
        }

        void CustomServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (var sc = new ServiceController(Program.InstallServiceName))
            {
                sc.Start();
            }
        }

    }
}
