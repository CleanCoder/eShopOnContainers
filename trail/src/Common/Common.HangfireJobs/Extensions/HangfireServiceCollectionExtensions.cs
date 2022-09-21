using Hangfire;
using ID.eShop.API.Common.Services.BackgroundJobs;
using ID.eShop.Common.HangfireJobs.Internal;
using ID.eShop.Common.HangfireJobs.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ID.eShop.Common.HangfireJobs.Extensions
{
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Hangfire contrib extensions and configures Hangfire with the specified <see cref="IGlobalConfiguration"/> action.
        /// </summary>
        public static IServiceCollection AddHanfire(this IServiceCollection services, Action<IGlobalConfiguration> configAction, bool playAsServer = false)
        {
            services.AddSingleton(services);

            services.AddHangfire(c => { configAction?.Invoke(c); });

            services.AddTransient<IBackgroundJobManager, BackgroundJobManager>();
            services.AddSingleton<BackgroundJobCollection>();

            if (playAsServer)
                services.AddHangfireServer();

            return services;
        }
    }
}
