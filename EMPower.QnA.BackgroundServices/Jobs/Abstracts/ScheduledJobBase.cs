using EMPower.QnA.BackgroundServices.Utils;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Abstracts
{
    /// <summary>
    /// A class base for all jobs that are scheduled to run at some time in the future.
    /// </summary>
    public abstract class ScheduledJobBase : JobBase, IJob
    {
        private JobDetailImpl _jobDetail;
        private ITrigger _trigger;

        public override IJobDetail JobDetail
        {
            get { return _jobDetail; }
        }

        public override ITrigger Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }

        public string JobFriendlyName { get; set; }

        public DateTime StartAt { get; set; }

        public sealed override void Initialize()
        {
            var jobIdentity = Guid.NewGuid().ToString();

            //If the start time is specified as
            if (StartAt < DateTime.Now)
            {
                StartAt = DateTime.Now.AddSeconds(1);
            }
            if (string.IsNullOrWhiteSpace(JobFriendlyName))
            {
                JobFriendlyName = jobIdentity;
            }

            _jobDetail = new JobDetailImpl(jobIdentity, GetType());
            _trigger = BuildTrigger();
        }

        public void Execute(IJobExecutionContext context)
        {
            JobFriendlyName = string.IsNullOrWhiteSpace(JobFriendlyName) ? Guid.NewGuid().ToString() : JobFriendlyName;

            ConfigurationManager.RefreshSection("appSettings");
            try
            {
                SLogger.Info(string.Format("Job {0} starts executing at {1}. Currently logged in as {2}.", JobFriendlyName, DateTime.Now, SystemReader.GetWindowsUsername()));

                //Check if the job is marked as disabled in the config
                if (!string.IsNullOrWhiteSpace(ConfigReader.DisableJobsByFriendlyName) && ConfigReader.DisableJobsByFriendlyName.Split(';').Any(j => j.Trim().Equals(JobFriendlyName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SLogger.Info(string.Format("Job {0} has been marked as disabled in the config file, therefore it does not execute this time.", JobFriendlyName));
                }
                else
                {
                    SLogger.Info(string.Format("Job {0} is executing...", JobFriendlyName));
                    InnerExcute(context);
                }
                SLogger.Info(string.Format("Job {0} finishes executing at {1}. Currently logged in as {2}.", JobFriendlyName, DateTime.Now, SystemReader.GetWindowsUsername()));
            }
            catch (Exception ex)
            {
                SLogger.Error(ex);
                SLogger.Info(string.Format("Job {0} stops executing at {1} due to internal errors. Currently logged in as {2}.", JobFriendlyName, DateTime.Now, SystemReader.GetWindowsUsername()));
            }

        }
        protected abstract ITrigger BuildTrigger();

        protected abstract void InnerExcute(IJobExecutionContext context);
    }
}
