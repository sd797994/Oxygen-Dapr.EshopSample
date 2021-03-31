using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.CronJob
{
    public abstract class CronJobBase
    {
        public abstract void RunCornJob();
    }
}
