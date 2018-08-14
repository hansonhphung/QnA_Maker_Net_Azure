using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMPower.QnA.Common.CommonDTO;
using EMPower.QnA.Common.Constants;
using EMPower.QnA.DTO.Models;
using EMPower.QnA.DTO.QnADTO;
using EMPower.QnA.WebApi.StandAlone.Constant;
using log4net;
using Newtonsoft.Json;

namespace EMPower.QnA.WebApi.StandAlone.Helper
{
    public class QnAHelper
    {
        private readonly string RUNNING = "Running";
        private readonly string NOT_START = "NotStarted";
        private readonly string LOCATION = "Location";
        private readonly string RETRY_AFTER = "Retry-After";
        private readonly string METHOD_POST = "POST";
        private readonly string METHOD_PATCH = "PATCH";

        private readonly ILog _loggerManager = LogManager.GetLogger("QnAHelperLog");
        public string QnAHost { get; set; }
        public string QnAKey { get; set; }
        public string QnAKnowledgeBaseId { get; set; }
        public string QnAService { get; set; }
        

        public ProxyDto ProxyObj { get; set; }

        //Get Question From Knowledge Base Method
        private const string MeThodGetQuestionFromKB = "/knowledgebases/{0}/test/qna/";
        private const string MeThodUpdateKnowledgeBase = "/knowledgebases/{0}";


        /// <summary>
        /// Get knowledge base from QnA Maker and return data
        /// </summary>
        /// <param name="knowledgeBaseId">QnA Knowleade Id</param>
        /// <returns>All QnA from specific Knowledge Base</returns>
        public async Task<GetQnAKnowledgeBaseResult> GetAllQnAFromKB(string knowledgeBaseId)
        {
            var meThodGetQuestionFromKB = String.Format(MeThodGetQuestionFromKB, QnAKnowledgeBaseId);
            var uri = QnAHost + QnAService + meThodGetQuestionFromKB;

            _loggerManager.Info("[GetQnAKnowledgeBases] executing....");

            var response = await MakeJsonRequestAndGetResponse(uri);

            if (CheckResponse(response))
            {
                _loggerManager.Info("[GetQnAKnowledgeBases] success....");
            }
            else
            {
                _loggerManager.Info("[GetQnAKnowledgeBases] got problems....");
            }

            var result = JsonConvert.DeserializeObject<GetQnAKnowledgeBaseResult>(response.Message);

            _loggerManager.Info("[GetQnAKnowledgeBases] done....");

            return result;
        }

        /// <summary>
        /// Get result from vision 6 and return data
        /// </summary>
        /// <param name="knowledgeBaseId">QnA Knowleade Id</param>
        /// <param name="HeatQuestion">List of new/updated questions from HEAT</param>
        /// <param name="QnAQuestion">List of current questions from QnA Maker</param>
        /// <returns>All QnA from specific Knowledge Base</returns>
        public async Task<bool> PushLatestQuestionToQnA(string knowledgeBaseId, List<QuestionsAndAnswers> HeatQuestion, List<QnAQuestionDto> QnAQuestion)
        {
            var newQuestion = HeatQuestion.Where(x => x.Status == QuestionStatus.PUBLISHED && QnAQuestion.All(y => GetHeatIdFromQnAQuestion(y.metadata) != x.HeatQuestionId.ToLower())).ToList();

            var updatedQuestion = HeatQuestion
                                 .Where(x => x.Status == QuestionStatus.PUBLISHED && QnAQuestion.Any(y => GetHeatIdFromQnAQuestion(y.metadata) == x.HeatQuestionId.ToLower()))
                                 .Select(q => GetUpdatedQuestionInfo(q, QnAQuestion))
                                 .ToList();

            var deletedQuestion = HeatQuestion
                                 .Where(x => x.Status == QuestionStatus.ARCHIVED && QnAQuestion.Any(y => GetHeatIdFromQnAQuestion(y.metadata) == x.HeatQuestionId.ToLower()))
                                 .Select(q => GetDeletedQuestionInfo(q, QnAQuestion))
                                 .ToList();

            var data = BuildAddOrUpdateOrDeleteQnADto(newQuestion, updatedQuestion, deletedQuestion);

            if (data.add == null && data.update == null && data.delete == null)
            {
                _loggerManager.Info("[UpdateKnowledgeBase] There is no new/updated question");
                return await Task.FromResult(true);
            }

            var meThodGetQuestionFromKB = String.Format(MeThodUpdateKnowledgeBase, QnAKnowledgeBaseId);
            var uri = QnAHost + QnAService + meThodGetQuestionFromKB;


            _loggerManager.Info("[UpdateKnowledgeBase] executing....");

            string body = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var response = await PostAsyncAndGetResponse(uri, body, METHOD_PATCH);

            await RetrySendRequest(WebApiConstants.HttpRequestRetryTimes, response);

            if (CheckResponse(response))
            {
                _loggerManager.Info("[UpdateKnowledgeBase] success....");

                return await Task.FromResult(true);
            }
            else
            {
                _loggerManager.Info("[UpdateKnowledgeBase] got problems....");
            }

            _loggerManager.Info("[UpdateKnowledgeBase] done....");

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Pushblish QnA Maker Knowledge base after training
        /// </summary>
        /// <param name="knowledgeBaseId">QnA Knowleade Id</param>
        public async Task<bool> PublishQnAKnowledgeBase(string knowledgeBaseId)
        {
            var meThodPublishKnowledgeBase = String.Format(MeThodUpdateKnowledgeBase, QnAKnowledgeBaseId);
            var uri = QnAHost + QnAService + meThodPublishKnowledgeBase;


            _loggerManager.Info("[PublishKnowledgeBase] executing....");

            var response = await PostAsyncAndGetResponse(uri, string.Empty, METHOD_POST);

            if (CheckResponse(response))
            {
                _loggerManager.Info("[PublishKnowledgeBase] success....");

                return await Task.FromResult(true);
            }
            else
            {
                _loggerManager.Info("[PublishKnowledgeBase] got problems....");
            }

            _loggerManager.Info("[UpdateKnowledgeBase] done....");

            return await Task.FromResult(false);
        }

        private QuestionsAndAnswers GetUpdatedQuestionInfo(QuestionsAndAnswers question, List<QnAQuestionDto> qnaQuestions)
        {
            question = qnaQuestions.Where(x => GetHeatIdFromQnAQuestion(x.metadata) == question.HeatQuestionId.ToLower())
                                   .Select(q => new QuestionsAndAnswers() {
                                       HeatQuestionId = question.HeatQuestionId,
                                       QnAQuestionId = q.id,
                                       HeatQuestion = question.HeatQuestion,
                                       QnAQuestion = q.questions.ToList(),
                                       Answer = question.Answer
                                   }).FirstOrDefault();
            return question;
        }

        private int GetDeletedQuestionInfo(QuestionsAndAnswers question, List<QnAQuestionDto> qnaQuestions)
        {
            return qnaQuestions.Where(x => GetHeatIdFromQnAQuestion(x.metadata) == question.HeatQuestionId.ToLower())
                                   .Select(q => q.id).FirstOrDefault();
        }

        private string GetHeatIdFromQnAQuestion(QnAQuestionMetaData[] metaData)
        {
            return metaData.Where(x => x.name == "heatquestionid").Select(q => q.value).FirstOrDefault();
        }

        private AddOrUpdateQnADto BuildAddOrUpdateOrDeleteQnADto(List<QuestionsAndAnswers> newQuestion, List<QuestionsAndAnswers> updatedQuestion, List<int> deletedQuestion)
        {
            var data = new AddOrUpdateQnADto()
            {
                add = BuildAddQnADto(newQuestion),
                update = BuildUpdateQnADto(updatedQuestion),
                delete = BuildDeleteQnADto(deletedQuestion)
            };
             
            return data;
        }

        private AddQnADto BuildAddQnADto(List<QuestionsAndAnswers> newQuestions)
        {
            if (newQuestions.Count > 0)
            {
                return new AddQnADto()
                {
                    qnaList = newQuestions.Select(x => new QnAQuestionDto()
                    {
                        id = x.QnAQuestionId,
                        answer = x.Answer,
                        questions = new List<string>() { x.HeatQuestion }.ToArray(),
                        metadata = new List<QnAQuestionMetaData>()
                                                                          { new QnAQuestionMetaData() {
                                                                                    name = "heatquestionid",
                                                                                    value = x.HeatQuestionId.ToString()
                                                                                }
                                                                          }.ToArray()
                    }).ToList()
                };
            }

            return null;
        }

        private UpdateQnADto BuildUpdateQnADto(List<QuestionsAndAnswers> updatedQuestions)
        {
            if (updatedQuestions.Count > 0)
            {
                return new UpdateQnADto()
                {
                    qnaList = updatedQuestions.Select(x => new UpdateQnAQuestionDto() {
                        id = x.QnAQuestionId,
                        answer = x.Answer,
                        questions = new UpdatedQuestionDto() {
                            delete = (x.QnAQuestion.Count == 0 || x.QnAQuestion.Contains(x.HeatQuestion)) ? null : x.QnAQuestion,
                            add = (x.QnAQuestion.Contains(x.HeatQuestion)) ? null : new List<string>() { x.HeatQuestion}
                        },
                    }).ToList()
                };
            }

            return null;
        }

        private DeleteQnADto BuildDeleteQnADto(List<int> deletedQuestions)
        {
            if (deletedQuestions.Count > 0)
            {
                return new DeleteQnADto
                {
                    ids = deletedQuestions
                };
            }

            return null;
        }

        /// <summary>
        /// Execute Json String to call API
        /// </summary>
        /// <param name="requestStr">request String</param>
        /// <returns></returns>
        private async Task<QnAResponseDto> MakeJsonRequestAndGetResponse(string requestStr)
        {
            var client = new HttpClient();

            // Is Use Proxy or not
            if (ProxyObj.UseInternetProxy)
            {
                var httpClienHandler = new HttpClientHandler()
                {
                    Proxy = new WebProxy(ProxyObj.InternetProxy)
                    {
                        Credentials = new NetworkCredential(
                        ProxyObj.SharedAccount,
                        ProxyObj.SharedPassword,
                        ProxyObj.SharedDomain)
                    }
                };

                client = new HttpClient(httpClienHandler);
            }

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(requestStr);
                request.Headers.Add("Ocp-Apim-Subscription-Key", QnAKey);
                

                var response = await client.SendAsync(request);

                return new QnAResponseDto()
                {
                    Headers = response.Headers,
                    Message = await response.Content.ReadAsStringAsync()
                };
            }
        }

        /// <summary>
        /// Execute Json String to call API
        /// </summary>
        /// <param name="requestStr">request String</param>
        /// <returns></returns>
        private async Task<QnAResponseDto> PostAsyncAndGetResponse(string requestStr, string body, string method)
        {
            var client = new HttpClient();

            // Is Use Proxy or not
            if (ProxyObj.UseInternetProxy)
            {
                var httpClienHandler = new HttpClientHandler()
                {
                    Proxy = new WebProxy(ProxyObj.InternetProxy)
                    {
                        Credentials = new NetworkCredential(
                        ProxyObj.SharedAccount,
                        ProxyObj.SharedPassword,
                        ProxyObj.SharedDomain)
                    }
                };

                client = new HttpClient(httpClienHandler);
            }

            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod(method);
                request.RequestUri = new Uri(requestStr);
                request.Headers.Add("Ocp-Apim-Subscription-Key", QnAKey);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");


                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();


                return new QnAResponseDto()
                {
                    Headers = response.Headers,
                    Message = responseBody
                };
            }
        }

        /// <summary>
        /// Check response If success, if not log it
        /// </summary>
        /// <param name="jsonStr">request Json String</param>
        /// <param name="logger">Logger</param>
        /// <returns>Is Success or Not</returns>
        private bool CheckResponse(QnAResponseDto response)
        {
            

            var responseData = JsonConvert.DeserializeObject<QnAResponseDto>(response.Message);
            if (responseData == null || responseData.error == null)
            {
                return true;
            }
            _loggerManager.Info("Error while Checking Response - " + response.error);
            return false;
        }

        private async Task RetrySendRequest(int times, QnAResponseDto response)
        {
            var operation = response.Headers.GetValues(LOCATION).First();

            var done = false;
            var retries = 0;
            while (true != done && retries < times)
            {
                var uri = QnAHost + QnAService + operation;

                response = await MakeJsonRequestAndGetResponse(uri);
                
                var responseState = JsonConvert.DeserializeObject<QnAResponseDto>(response.Message);

                string state = responseState.operationState;

                if (state.CompareTo(RUNNING) == 0 || state.CompareTo(NOT_START) == 0)
                {
                    var wait = response.Headers.GetValues(RETRY_AFTER).First();

                    _loggerManager.Info("Waiting " + wait + " seconds...");

                    Thread.Sleep(Int32.Parse(wait) * 1000);
                }
                else
                {
                    done = true;
                    _loggerManager.Info(string.Format("Request sent successfully after  retrying {0} times", retries));
                }
            }
        }
    }
}
