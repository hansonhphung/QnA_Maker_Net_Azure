using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using EMPower.QnA.BackgroundServices.Jobs.Abstracts;
using EMPower.QnA.BackgroundServices.Utils;
using EMPower.QnA.Data.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EMPower.QnA.BackgroundServices.Constants;
using EMPower.QnA.DTO.Models;
using EMPower.QnA.DTO.QnADTO;
using EMPower.QnA.BackgroundServices.Abstracts;

namespace EMPower.QnA.BackgroundServices.Jobs.Implements
{
    /// <summary>
    /// Check and resend approval reminder emails to approvers
    /// </summary>
    [DisallowConcurrentExecution]
    public class PushLatestQnAJob : ScheduledJobBase, IJob
    {
        protected readonly IQnAServices _qnaService;

        public PushLatestQnAJob(IQnAServices qnaService)
        {
            JobFriendlyName = "PushLatestQnAJob";
            _qnaService = qnaService;
        }

        protected sealed override ITrigger BuildTrigger()
        {
            var triggerIdentity = Guid.NewGuid().ToString();
            return TriggerBuilder
                .Create()
                .WithIdentity(triggerIdentity, "Interval Jobs")
                .StartNow()
                //.WithSimpleSchedule(x => x.WithIntervalInMinutes(ConfigReader.PushLatestQnAJobMinutesInterval)
                //.RepeatForever())
                .WithCronSchedule(ConfigReader.InitialisePushLatestQnAJobCronTrigger)
                .Build();
        }

        protected override void InnerExcute(IJobExecutionContext context)
        {
            try
            {
                SLogger.Info(string.Format("{0}: Starting Push question to QnA job", JobFriendlyName));

                var lastRunTime = SystemReader.GetLastPushQnAJobRunTime();

                var lstQuestion = _qnaService.GetQnAByDate(lastRunTime);

                // call stand alone to execute QnA
                var data = new PushLatestQnA { ListQuestionAndAnswers = lstQuestion.ToList() };

                ApiHelper.PostAsyncNoEncrypt(WebApiConstant.XoomPushQuestionStandAloneEndPoint, data).GetAwaiter().GetResult();

                SLogger.Info(string.Format("{0} : - End Push question to QnA job", JobFriendlyName));

                SystemReader.SetLastPushQnAJobRunTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                SLogger.Error(string.Format("{0}: InnerExcute. Push question to QnA job", JobFriendlyName), ex);
            }
        }
    }
}
