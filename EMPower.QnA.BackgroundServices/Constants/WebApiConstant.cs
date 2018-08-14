using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Constants
{
    public class WebApiConstant
    {
        private static string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string StandAloneBaseApi
        {
            get { return ReadString("StandAloneBaseApi"); }
        }

        public static string XoomPushQuestionStandAloneEndPoint
        {
            get { return ReadString("XoomPushQuestionStandAloneEndPoint"); }
        }
    }
}
