using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAPITest.Databases;
using EFAPITest.Managers;
using EFAPITest.Model;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        ILogger _logger;
        JobMgr _jobMgr;

        public JobsController(ILogger<JobsController> logger, JobMgr jobMgr)
        {
            _logger = logger;
            _jobMgr = jobMgr;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetAsync()
        {
            var jobs = await _jobMgr.GetJobs();
            return Ok(jobs);
        }
        
        [HttpGet("{JMPID}")]
        public async Task<ActionResult<Job>> GetAsync(int JMPID)
        {
            var jobs = await _jobMgr.GetJobs();
            var jobsWithMatchingJMPID = jobs.Where(j => j.JMPID == JMPID);
            return jobsWithMatchingJMPID.FirstOrDefault();
        }
        
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Job newJob)
        {
            // Check if a record with this JMPID already exists.
            var jobs = await _jobMgr.GetJobs();
            var jobsWithMatchingJMPID = jobs.Where(j => j.JMPID == newJob.JMPID);
            if (jobsWithMatchingJMPID.Count() > 0)
            {
                _logger.LogInformation("Unable to create Job, JMPID already in use: {JMPID}", newJob.JMPID);
                return NotFound("JMPID already exists");
            }

            await _jobMgr.CreateJob(newJob);

            return Ok();
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }


        [Route("Convert")]
        [HttpPost]
        public void ConvertOldJobs()
        {
            BackgroundJob.Enqueue(() => _jobMgr.ConvertAllOldJobs(JobCancellationToken.Null));
        }
    }
}
