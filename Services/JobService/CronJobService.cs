using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobService
{
    public class CronJobService : IHostedService
    {
        public CronJobService()
        {
            CornJobRunnerFactory.RegisterAllJobs();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            CornJobRunnerFactory.RunAllJobs();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
