using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EMPower.QnA.Common.CommonDTO;
using EMPower.QnA.WebApi.StandAlone.Constant;

namespace EMPower.QnA.WebApi.StandAlone.Helper
{
    public class ProxyHelper
    {
        public static ProxyDto GetProxyForQnA()
        {
            return new ProxyDto
            {
                UseInternetProxy = WebApiConstants.UseInternetProxy,
                InternetProxy = WebApiConstants.InternetProxy,
                SharedAccount = WebApiConstants.SharedAccount,
                SharedDomain = WebApiConstants.SharedDomain,
                SharedPassword = WebApiConstants.SharedPassword
            };
        }
        public static WebClient CreateWebClient(SecurityProtocolType securityType = SecurityProtocolType.Tls, bool bypassSsl = true)
        {
            if (bypassSsl)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => true;
            }

            if (securityType != SecurityProtocolType.Tls)
            {
                ServicePointManager.SecurityProtocol = securityType;
            }

            var webClient = new WebClient();
            if (WebApiConstants.UseInternetProxy)
            {
                webClient.Proxy = new WebProxy(WebApiConstants.InternetProxy)
                {
                    Credentials = new NetworkCredential(WebApiConstants.SharedAccount,
                        WebApiConstants.SharedPassword,
                        WebApiConstants.SharedDomain)
                };
            }
            return webClient;
        }
    }
}