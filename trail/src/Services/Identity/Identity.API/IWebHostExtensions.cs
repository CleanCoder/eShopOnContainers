using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class IWebHostExtensions
    {
        public static bool IsInKubernetes(this IWebHost webHost)
        {
            var cfg = webHost.Services.GetService<IConfiguration>();
            var orchestratorType = cfg.GetValue<string>("OrchestratorType");
            return orchestratorType?.ToUpper() == "K8S";
        }

        public static bool IsInMemoryDB(this IWebHost webHost)
        {
            var cfg = webHost.Services.GetService<IConfiguration>();
            return cfg.GetValue<bool>("UseInMemoryDB");
        }

            public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            var underK8s = webHost.IsInKubernetes();
            var isInMemoryDB = webHost.IsInMemoryDB();

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    if (underK8s)
                    {
                        InvokeSeeder(seeder, context, services, isInMemoryDB);
                    }
                    else
                    {
                        var retries = 10;
                        var retry = Policy.Handle<Microsoft.Data.SqlClient.SqlException>()
                            .WaitAndRetry(
                                retryCount: retries,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (exception, timeSpan, retry, ctx) =>
                                {
                                    logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", nameof(TContext), exception.GetType().Name, exception.Message, retry, retries);
                                });

                        //if the sql server container is not created on run docker compose this
                        //migration can't fail for network related exception. The retry options for DbContext only 
                        //apply to transient exceptions
                        // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                        retry.Execute(() => InvokeSeeder(seeder, context, services, isInMemoryDB));
                    }

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (underK8s)
                    {
                        throw;          // Rethrow under k8s because we rely on k8s to re-run the pod
                    }
                }
            }

            return webHost;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services, bool isInMemoryDb)
            where TContext : DbContext
        {
            if (!isInMemoryDb)
            {
                context.Database.Migrate();
            }

            seeder(context, services);
        }
    }
}
