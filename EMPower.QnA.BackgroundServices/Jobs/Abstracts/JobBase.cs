using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.BackgroundServices.Abstracts
{
    public abstract class JobBase
    {
        protected static ILog SLogger = LogManager.GetLogger(typeof(JobBase));

        public abstract IJobDetail JobDetail { get; }
        public abstract ITrigger Trigger { get; set; }
        public bool IsExcluded { get; set; }

        public abstract void Initialize();
    }
}
