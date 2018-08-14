using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using EMPower.QnA.Data.Abstractions;
using EMPower.QnA.DTO.Models;
using EMPower.QnA.DTO.QnADTO;
using EMPower.QnA.WebApi.StandAlone.Constant;
using EMPower.QnA.WebApi.StandAlone.Helper;
using log4net;
using Newtonsoft.Json;

namespace EMPower.QnA.WebApi.StandAlone.Controllers
{
    public class QnAController : BaseController
    {
        protected readonly IQnAServices _qnaService;

        public QnAController(IQnAServices qnaService)
        {
            _qnaService = qnaService;
        }

        // POST: api/qna/pushlatestqna
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("pushlatestqna")]
        public async Task<bool> PushLatestQnA(PushLatestQnA data)
        {
            QnAHelper qnaHelper = new QnAHelper {
                ProxyObj = ProxyHelper.GetProxyForQnA(),
                QnAHost = WebApiConstants.QnAHost,
                QnAKey = WebApiConstants.QnAKey,
                QnAService = WebApiConstants.QnAService,
                QnAKnowledgeBaseId = WebApiConstants.QnAKnowledgeBaseId
            };

            var lstQnaQuestion = await qnaHelper.GetAllQnAFromKB(WebApiConstants.QnAKnowledgeBaseId);

            logger.Info("Start Push QnAs to QnA Maker");

            var result = await qnaHelper.PushLatestQuestionToQnA(WebApiConstants.QnAKnowledgeBaseId, data.ListQuestionAndAnswers, lstQnaQuestion.qnaDocuments);

            logger.Info("Finish Push QnAs to QnA Maker");

            if (result)
            {
                logger.Info("Start Publish QnA Knowledge Base");

                result = await qnaHelper.PublishQnAKnowledgeBase(WebApiConstants.QnAKnowledgeBaseId);

                logger.Info("Finish Publish QnA Knowledge Base");
            }

            return await Task.FromResult(result);
        }


        // POST: api/qna/getqnaknowledgebase
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("getqnaknowledgebase")]
        public async Task<GetQnAKnowledgeBaseResult> GetQnAKnowledgeBase()
        {
            QnAHelper qnaHelper = new QnAHelper
            {
                ProxyObj = ProxyHelper.GetProxyForQnA(),
                QnAHost = WebApiConstants.QnAHost,
                QnAKey = WebApiConstants.QnAKey,
                QnAService = WebApiConstants.QnAService,
                QnAKnowledgeBaseId = WebApiConstants.QnAKnowledgeBaseId
            };

            logger.Info("Start get QnAs from QnA Maker");

            var lstQuestionResponseString = await qnaHelper.GetAllQnAFromKB(WebApiConstants.QnAKnowledgeBaseId);

            logger.Info("Finish get QnAs from QnA Maker");

            return await Task.FromResult<GetQnAKnowledgeBaseResult>(lstQuestionResponseString);
        }

        // POST: api/qna/publishqnaknowledgebase
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("publishqnaknowledgebase")]
        public async Task<bool> PublishQnAKnowledgeBase()
        {
            QnAHelper qnaHelper = new QnAHelper
            {
                ProxyObj = ProxyHelper.GetProxyForQnA(),
                QnAHost = WebApiConstants.QnAHost,
                QnAKey = WebApiConstants.QnAKey,
                QnAService = WebApiConstants.QnAService,
                QnAKnowledgeBaseId = WebApiConstants.QnAKnowledgeBaseId
            };

            logger.Info("Start Publish QnA Knowledge Base");

            var result = await qnaHelper.PublishQnAKnowledgeBase(WebApiConstants.QnAKnowledgeBaseId);

            logger.Info("Finish Publish QnA Knowledge Base");

            return await Task.FromResult(result);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult test() {
            return Ok("TEST");
        }
    }
}
