using Biz.API.Jobs;
using ID.eShop.API.Common.Services.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Biz.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobTestController : ControllerBase
    {
        private readonly ILogger<JobTestController> _logger;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private static Random _random = new Random();


        public JobTestController(IBackgroundJobManager backgroundJobManager, ILogger<JobTestController> logger)
        { 
            _backgroundJobManager = backgroundJobManager;
            _logger = logger;
        }

        [HttpGet("trigger")]
        public async Task<IActionResult> TriggleNotification()
        {
            await _backgroundJobManager.EnqueueAsync(new NotificationJobArgs(_random.Next()));
            return Ok();
        }
    }
}
