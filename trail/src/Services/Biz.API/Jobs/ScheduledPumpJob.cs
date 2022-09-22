using Hangfire;
using ID.eShop.API.Common.Constants;
using ID.eShop.API.Common.Services.BackgroundJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Biz.API.Jobs
{
    public class ScheduledPumpJob : ScheduledJob
    {
        private readonly ILogger<ScheduledPumpJob> _logger;

        public ScheduledPumpJob(ILogger<ScheduledPumpJob> logger) :base()
        {
            _logger = logger;
        }

        public override string Schedule => CommonCrons.Minutely;

        public override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Pump @{DateTime.Now.ToString()}");
            return Task.CompletedTask;
        }
    }
}
