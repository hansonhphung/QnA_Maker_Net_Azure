using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Utils
{
    public static class SystemReader
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SystemReader));
        private static readonly string LAST_SERVICE_RUN_TIME_FILE = @"LastServiceRunTime.txt";

        /// <summary>
        /// Gets the username including domain of the user currently logged in
        /// </summary>
        /// <returns>The username of the currently logged in user</returns>
        public static string GetWindowsUsername()
        {
            var username = string.Empty;
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
                var collection = searcher.Get();
                username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return username;
        }

        /// <summary>
        /// Gets the environment username of the user currently logged in
        /// </summary>
        /// <returns>The username of the currently logged in user</returns>
        public static string GetEnvironmentUsername()
        {
            var username = string.Empty;
            try
            {
                var windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                {
                    username = windowsIdentity.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return username;
        }

        /// <summary>
        /// Gets last Push QnA Job run time
        /// </summary>
        /// <returns>The last date time when Push QnA Job run </returns>
        public static DateTime GetLastPushQnAJobRunTime()
        {
            var date = DateTime.Now;

            if (File.Exists(LAST_SERVICE_RUN_TIME_FILE))
            {
                using (StreamReader sr = new StreamReader(LAST_SERVICE_RUN_TIME_FILE))
                {
                    var isSuccess = DateTime.TryParse(sr.ReadLine(), out date);
                    sr.Close();

                    return isSuccess ? date : DateTime.Now;
                }
            }

            return date;
        }

        /// <summary>
        /// Sets last Push QnA Job run time
        /// </summary>
        /// <returns>The last date time when Push QnA Job run </returns>
        public static void SetLastPushQnAJobRunTime(DateTime date)
        {
            using (var fileStream = new FileStream(LAST_SERVICE_RUN_TIME_FILE, FileMode.OpenOrCreate))
            using (var sw = new StreamWriter(fileStream))
            {
                sw.WriteLine(date.ToString());
                sw.Close();
            }
        }
    }
}
