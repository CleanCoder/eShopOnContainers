using ID.eShop.API.Common.Services.BackgroundJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ID.eShop.Common.HangfireJobs.Authorization
{
    /// <summary>
    /// Represents options for Hangfire basic authentication
    /// </summary>
    public class BasicAuthAuthorizationFilterOptions
    {
        public BasicAuthAuthorizationFilterOptions()
        {
            LoginCaseSensitive = true;
            Users = new BasicAuthAuthorizationUser[] { };
        }

        /// <summary>
        /// Whether or not login checking is case sensitive.
        /// </summary>
        public bool LoginCaseSensitive { get; set; }

        /// <summary>
        /// Represents users list to access Hangfire dashboard.
        /// </summary>
        public IEnumerable<BasicAuthAuthorizationUser> Users { get; set; }
    }
}
