using EMPower.QnA.BackgroundServices.Abstracts;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Jobs
{
    /// <summary>
    /// A Job Manager that manages all the jobs in the Windows Service
    /// </summary>
    public class ScheduleJobManager
    {
        public IScheduler Scheduler;

        /// <summary>
        /// Initializes the job manager.
        /// </summary>
        public ScheduleJobManager(IJobFactory factory)
        {
            Scheduler = new StdSchedulerFactory().GetScheduler();
            Scheduler.JobFactory = factory;
        }

        /// <summary>
        /// Adds a new job to the job manager.
        /// </summary>
        /// <typeparam name="T">The type of the job, must inherit from JobBase</typeparam>
        /// <param name="task">An instance of that type</param>
        public void AddJob<T>(T task) where T : JobBase
        {
            task.Initialize();
            Scheduler.ScheduleJob(task.JobDetail, task.Trigger);
        }

        /// <summary>
        /// Starts the job scheduler.
        /// </summary>
        public void Start()
        {
            Scheduler.Start();
        }

        /// <summary>
        /// Stops the job scheduler.
        /// </summary>
        public void Stop()
        {
            Scheduler.Shutdown();
        }
    }
}
