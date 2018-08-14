using EMPower.QnA.BackgroundServices.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Utils
{
    public static class ApiHelper
    {
        public static void Post(string apiEndPoint, NameValueCollection data)
        {
            InvokeApi(apiEndPoint, data);
        }

        public static TResult Post<TResult>(string apiEndPoint, NameValueCollection data)
        {
            var responseArr = InvokeApi(apiEndPoint, data);
            var encodedRequest = Encoding.Default.GetString(responseArr);
            return JsonConvert.DeserializeObject<TResult>(encodedRequest);
        }

        private static byte[] InvokeApi(string apiEndPoint, NameValueCollection data)
        {
            using (var webClient = ProxyHelper.CreateWebClient())
            {
                try
                {
                    var responseData = webClient.UploadValues(apiEndPoint, "POST", data);
                    return responseData;
                }
                catch (WebException ex)
                {
                    if (ex.Status != WebExceptionStatus.ProtocolError)
                        throw;

                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        var responseText = GetApiResponseString(response);
                        var message = string.Format("Invoke api failed. Status Code: {0}-{1}. Response:  {2}", response.StatusCode, response.StatusDescription, responseText);
                        throw new Exception(message, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static TResult Get<TResult>(string endPoint)
        {
            using (var webClient = ProxyHelper.CreateWebClient())
            {
                try
                {
                    var responseString = webClient.DownloadString(endPoint);
                    return JsonConvert.DeserializeObject<TResult>(responseString);
                }
                catch (WebException ex)
                {
                    if (ex.Status != WebExceptionStatus.ProtocolError)
                        throw;

                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        var responseText = GetApiResponseString(response);
                        var message = string.Format("Invoke api failed. Status Code: {0}-{1}. Response:  {2}", response.StatusCode, response.StatusDescription, responseText);
                        throw new Exception(message, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task<string> GetAsync(string uri)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(uri);
                return result;
            }
        }

        public static async Task<string> PostAsyncNoEncrypt(string requestUri, object value)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebApiConstant.StandAloneBaseApi);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PostAsJsonAsync(requestUri, value);
                var objResult = await result.Content.ReadAsStringAsync();

                return objResult;
            }
        }

        
        public static T PostAsJson<T>(string requestUri, object value)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var content = new StringContent(value.ToString(), Encoding.UTF8, "application/json");
                var response = client.PostAsync(requestUri, content).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            }
        }

        private static string GetApiResponseString(HttpWebResponse response)
        {
            try
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
