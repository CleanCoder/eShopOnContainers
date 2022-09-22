using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ID.eShop.API.Common.Services.BackgroundJobs
{
    /// <summary>
    /// Base class for Hangfire jobs which provides auto-scheduling and logging.
    /// </summary>
    public abstract class ScheduledJob
    {
        /// <summary>
        /// Gets the <see cref="ILogger"/> instance for this job.
        /// </summary>
        public ILogger<ScheduledJob> Logger { get; set; }

        /// <summary>
        /// Gets the schedule for automatically scheduled jobs.
        /// Default is null which means that the job is not automatically scheduled.
        /// </summary>
        public virtual string Schedule => null;

        /// <summary>
        /// Initializes the <see cref="ScheduledJob"/>.
        /// </summary>
        protected ScheduledJob()
        {
            Logger = NullLogger<ScheduledJob>.Instance;
        }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    }
}
