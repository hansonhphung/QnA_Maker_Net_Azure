using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EMPower.QnA.WebApi.StandAlone.Constant
{
    public class WebApiConstants
    {
        public static string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static int ReadInt(string key)
        {
            return Int32.Parse(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// Is Use Proxy to Internet or not ?
        /// </summary>
        public static bool UseInternetProxy
        {
            get
            {
                return ReadString("UseInternetProxy").Equals("true");
            }
        }

        /// <summary>
        /// Internet Proxy
        /// </summary>
        public static string InternetProxy
        {
            get { return ReadString("InternetProxy"); }
        }

        /// <summary>
        /// Account to use Proxy
        /// </summary>
        public static string SharedAccount
        {
            get { return ReadString("SharedAccount"); }
        }

        /// <summary>
        /// Password to use Proxy
        /// </summary>
        public static string SharedPassword
        {
            get { return ReadString("SharedPassword"); }
        }

        /// <summary>
        /// Domain to use Proxy
        /// </summary>
        public static string SharedDomain
        {
            get { return ReadString("SharedDomain"); }
        }

        /// <summary>
        /// QnA Maker Host
        /// </summary>
        public static string QnAHost
        {
            get { return ReadString("QnAHost"); }
        }

        /// <summary>
        /// QnA Maker Key
        /// </summary>
        public static string QnAKey
        {
            get { return ReadString("QnAKey"); }
        }

        /// <summary>
        /// QnA Knowledge Base Id
        /// </summary>
        public static string QnAKnowledgeBaseId
        {
            get { return ReadString("QnAKnowledgeBaseId"); }
        }

        /// <summary>
        /// QnA Service
        /// </summary>
        public static string QnAService
        {
            get { return ReadString("QnAService"); }
        }

        /// <summary>
        /// HttpRequestRetryTimes
        /// </summary>
        public static int HttpRequestRetryTimes
        {
            get { return ReadInt("HttpRequestRetryTimes"); }
        }


    }
}