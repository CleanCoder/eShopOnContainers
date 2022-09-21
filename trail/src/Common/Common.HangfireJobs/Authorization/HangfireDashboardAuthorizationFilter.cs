using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Linq;

namespace ID.eShop.Common.HangfireJobs.Authorization
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly BasicAuthAuthorizationFilterOptions _options;

        public HangfireDashboardAuthorizationFilter() : this(new BasicAuthAuthorizationFilterOptions())
        {
        }

        public HangfireDashboardAuthorizationFilter(BasicAuthAuthorizationFilterOptions options)
        {
            _options = options;
        }
        private bool Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            return false;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            string header = httpContext.Request.Headers["Authorization"];

            try
            {
                if (string.IsNullOrWhiteSpace(header) == false)
                {
                    AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);

                    if ("Basic".Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase))
                    {
                        string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                        var parts = parameter.Split(':');

                        if (parts.Length > 1)
                        {
                            string login = parts[0];
                            string password = parts[1];

                            if (string.IsNullOrWhiteSpace(login) == false && string.IsNullOrWhiteSpace(password) == false)
                            {
                                return _options
                                    .Users
                                    .Any(user => user.Validate(login, password)) || Challenge(httpContext);
                            }
                        }
                    }
                }

                return Challenge(httpContext);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
