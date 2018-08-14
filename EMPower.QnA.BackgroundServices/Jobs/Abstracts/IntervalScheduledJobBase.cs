using EMPower.QnA.BackgroundServices.Abstracts;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Jobs.Abstracts
{
    /// <summary>
    /// A class base for all jobs scheduled to run at an interval.
    /// </summary>
    public abstract class IntervalScheduledJobBase : ScheduledJobBase, IJob
    {
        /// <summary>
        /// The time between to runs of the job.
        /// </summary>
        public int MinutesInterval { get; set; }

        protected sealed override ITrigger BuildTrigger()
        {
            //If the interval is not a valid time lapse, set it to 1 minute
            if (MinutesInterval <= 0)
            {
                MinutesInterval = 1;
            }

            var triggerIdentity = Guid.NewGuid().ToString();
            //return TriggerBuilder
            //    .Create()
            //    .WithIdentity(triggerIdentity, "Interval Jobs")
            //    .StartAt(StartAt)
            //    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInMinutes(MinutesInterval))
            //    .Build();
            return TriggerBuilder
                .Create()
                .WithIdentity(triggerIdentity, "Interval Jobs")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(MinutesInterval)
                .RepeatForever())
                .Build();
        }
    }
}
