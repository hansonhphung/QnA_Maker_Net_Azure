using EMPower.QnA.BackgroundServices.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EMPower.QnA.BackgroundServices.Jobs.Abstracts;

namespace EMPower.QnA.BackgroundServices.Jobs.Abstracts
{
    public abstract class TestJobBase : IntervalScheduledJobBase
    {
        #region fields
        //Injected services
        #endregion

        protected TestJobBase()
        {
            //Injected services
        }

        //Maximun number of contacts can auto send, don't need to approve, send to vision6
        protected virtual int MaxEmailAllowAutoSend { get; set; }

        public abstract string GetJobFriendlyName();
        
        protected override void InnerExcute(IJobExecutionContext context)
        {
            //CheckAndCreateEmailRolloutForNewContacts();
        }
        /// <summary>
        /// Update db when we cant execute to add contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <summary>
        /// Get rolling list from  Web API of Xoom
        /// </summary>
        /// <returns></returns>
        protected virtual int TestJob()
        {
            var guid = Guid.NewGuid().ToString();
            try
            {
                //////Debug.WriteLine(string.Format("{0} : {1} - Start check and create if any new contacts found.WebApiCreateNewRollOutForEndPoint: {2}", guid, GetJobFriendlyName(), WebApiCreateNewRollOutForEndPoint));
                ////SLogger.Info(string.Format("{0} : {1} - Start check and create if any new contacts found.WebApiCreateNewRollOutForEndPoint: {2}", guid, GetJobFriendlyName(), WebApiCreateNewRollOutForEndPoint));
                ////Debug.WriteLine(string.Format("{0} : {1} - Start check and create if any new contacts found.WebApiCreateNewRollOutForEndPoint: {2}", guid, GetJobFriendlyName(), WebApiCreateNewRollOutForEndPoint));
                //var rollOutReqParam = new RolloutEmailModel
                //{
                //    LimitAttempts = ConfigReader.RollOutMaxRetry,
                //    SelectTop = ConfigReader.RollOutMaxEmailPerRun,
                //    MaxEmailAllowAutoSend = ConfigReader.MaxAllowAutoSendEmail,
                //    MigrateStatus = MigrateStatus,
                //    SelectGroupOnly = SelectGroupOnly
                //};

                //var result = _rolloutEmailHelper.RolloutEmailsVision6(rollOutReqParam);

                //SLogger.Info(string.Format("{0} : {1} - End check and create if any new contacts found", guid, GetJobFriendlyName()));
                ////Debug.WriteLine(string.Format("{0} : {1} - End check and create if any new contacts found", guid, GetJobFriendlyName()));
                //if (result!= null && result.RolloutId> 0)
                //{
                //    SLogger.Info(string.Format("{0} : {1} -  New rollout emails found. Total emails: {2}. Allow auto send: {3}", guid, GetJobFriendlyName(), result.TotalRolloutContacts, result.AllowAutoSend));
                //    //Debug.WriteLine(string.Format("{0} : {1} -  New rollout emails found. Total emails: {2}. Allow auto send: {3}", guid, GetJobFriendlyName(), result.TotalRolloutContacts, result.AllowAutoSend));
                //}
                //else
                //{
                //    SLogger.Info(string.Format("{0} : {1} -  No new rollout emails found", guid, GetJobFriendlyName()));
                //    //Debug.WriteLine(string.Format("{0} : {1} -  No new rollout emails found", guid, GetJobFriendlyName()));
                //}
                //////Debug.WriteLine(string.Format("{0} : {1} - End check and create if any new contacts found.WebApiCreateNewRollOutForEndPoint: {2}", guid, GetJobFriendlyName(), WebApiCreateNewRollOutForEndPoint));
                //return result;
            }
            catch (Exception ex)
            {
                SLogger.Error(string.Format("{0} : {1} - EXCEPTION check and create if any new contacts found", guid, GetJobFriendlyName()), ex);
                //Debug.WriteLine(string.Format("{0} : {1} - EXCEPTION check and create if any new contacts found", guid, GetJobFriendlyName()));
            }
            return default(int);
        }        
    }
}
