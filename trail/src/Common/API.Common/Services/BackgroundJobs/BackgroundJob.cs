using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ID.eShop.API.Common.Services.BackgroundJobs
{
    public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
    {
        public ILogger<BackgroundJob<TArgs>> Logger { get; set; }

        protected BackgroundJob()
        {
            Logger = NullLogger<BackgroundJob<TArgs>>.Instance;
        }

        public abstract Task ExecuteAsync(TArgs args);
    }
}
