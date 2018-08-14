using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Utils
{
    public class ConfigReader
    {
        #region Helper Method
        private static string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #endregion

        #region General
        public static string DisableJobsByFriendlyName
        {
            get { return ReadString("DisableJobsByFriendlyName"); }
        }
        #endregion

        #region
        public static int PushLatestQnAJobMinutesInterval
        {
            get { return Int32.Parse(ReadString("PushLatestQnAJobMinutesInterval")); }
        }
        #endregion

        #region
        public static string InitialisePushLatestQnAJobCronTrigger
        {
            get { return ReadString("InitialisePushLatestQnAJobCronTrigger"); }
        }
        #endregion
    }
}

