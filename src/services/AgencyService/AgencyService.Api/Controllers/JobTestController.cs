using AgencyService.Application.Contracts.Services;
using BuildingBlocks.Api.Controllers;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Api.Controllers
{
    public class JobTestController : BaseController
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobTestController(
            IJobTestService jobTestService, 
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        [Route("fire-and-forget-job")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJobAsync());
            return Ok();
        }

        [HttpGet]
        [Route("delayed-job")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => 
                _jobTestService.DelayedJobAsync(), 
                TimeSpan.FromSeconds(60));
            
            return Ok();
        }

        [HttpGet]
        [Route("reccuring-job")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => 
                _jobTestService.ReccuringJobAsync(), 
                Cron.Minutely);
            
            return Ok();
        }

        [HttpGet]
        [Route("continuation-job")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => 
                _jobTestService.FireAndForgetJobAsync());
            
            _backgroundJobClient.ContinueJobWith(parentJobId, () => 
                _jobTestService.ContinuationJobAsync());

            return Ok();
        }
    }
}
    