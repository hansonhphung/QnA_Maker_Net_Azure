using System.Net;

namespace EMPower.QnA.BackgroundServices.Utils
{
    public class ProxyHelper
    {
        public static WebClient CreateWebClient()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            var webClient = new WebClient();
            return webClient;
        }
    }
}
