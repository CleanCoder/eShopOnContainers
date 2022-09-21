﻿using ID.eShop.API.Common.Services.BackgroundJobs;
using ID.eShop.API.Common.Util;
using System;

namespace Biz.API.Jobs
{

    /// <summary>
    /// Arguments for <see cref="NotificationJobArgs"/>.
    /// </summary>
    [Serializable]
    public class NotificationJobArgs
    {
        /// <summary>
        /// Notification Id.
        /// </summary>
        public long NotificationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationJobArgs"/> class.
        /// </summary>
        public NotificationJobArgs(long notificationId)
        {
            NotificationId = notificationId;
        }
    }

    public class NotificationJob : BackgroundJob<NotificationJobArgs>
    {
        public override void Execute(NotificationJobArgs args)
        {
            Console.WriteLine(args.NotificationId);
        }
    }
}
