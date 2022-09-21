using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ID.eShop.Common.HangfireJobs.Internal
{
    /// <summary>
    /// Defines options for the Hangfire configure.
    /// </summary>
    public class HangfireConfigureOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Hangfire background server should be enabled.
        /// </summary>
        public bool EnableServer { get; set; } = false;

        /// <summary>
        /// Gets or sets options for the Hangfire dashboard.
        /// </summary>
        public DasbhoardOptions Dasbhoard { get; set; } = new DasbhoardOptions();

        /// <summary>
        /// Defines options for the Hangfire dashboard.
        /// </summary>
        public class DasbhoardOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether the Hangfire dashboard should be enabled.
            /// </summary>
            public bool Enabled { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether the Hangfire dashboard IP-based authorization filter should be enabled.
            /// </summary>
            public bool EnableAuthorization { get; set; } = true;

            /// <summary>
            /// The dashboard path
            /// </summary>
            public string PathMatch { get; set; } = "/hangfire";

            /// <summary>
            /// The Title displayed on the dashboard, optionally modify to describe this dashboards purpose.
            /// </summary>
            public string DashboardTitle { get; set; } = "Hangfire Dashboard";
        }
    }
}
