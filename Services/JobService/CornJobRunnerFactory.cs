using JobService.CronJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService
{
    public class CornJobRunnerFactory
    {
        static List<Action> Jobs = new List<Action>();
        public static void RegisterAllJobs()
        {
            foreach (var childclass in typeof(CronJobBase).Assembly.GetTypes().Where(x => x.BaseType == typeof(CronJobBase)))
            {
                Action job = childclass.GetMethod(nameof(CronJobBase.RunCornJob)).CreateDelegate(typeof(Action), Activator.CreateInstance(childclass)) as Action;
                Jobs.Add(job);
            }
        }
        public static void RunAllJobs()
        {
            foreach (var job in Jobs)
            {
                job();
            }
        }
    }
}
