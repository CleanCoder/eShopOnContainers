using ID.eShop.Common.HangfireJobs.Authorization;
using ID.eShop.Common.HangfireJobs.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire
{
    public static class HangfireConfigureExtensions
    {
        public static void UseHangfire(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<HangfireConfigureOptions>>()?.Value ?? new HangfireConfigureOptions();

            if (options.EnableServer)
                app.ApplicationServices.GetRequiredService<IServiceCollection>().AddHangfireServer();

            if (options.Dasbhoard.Enabled)
            {
                var dashboardOptions = app.ApplicationServices.GetService<IOptions<DashboardOptions>>()?.Value ?? new DashboardOptions();
                dashboardOptions.DashboardTitle = options.Dasbhoard.DashboardTitle;
                if (options.Dasbhoard.EnableAuthorization)
                {
                    var dashboardAuthorizationFilter = new HangfireDashboardAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        LoginCaseSensitive = true,
                        Users = new[] { new BasicAuthAuthorizationUser { Login = "Admin", PasswordClear = "admin123" } }
                    });

                    dashboardOptions.Authorization = new[] { dashboardAuthorizationFilter };

                    app.UseHangfireDashboard(options.Dasbhoard.PathMatch, options: dashboardOptions);
                }
            }
        }
    }
}
