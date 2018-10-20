using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAPITest.Databases;
using EFAPITest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        ILogger _logger;
        MainDBContext _mainDBContext;

        public JobsController(ILogger<JobsController> logger, MainDBContext mainDBContext)
        {
            _logger = logger;
            _mainDBContext = mainDBContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Job>> Get()
        {
            return Ok(_mainDBContext.Jobs);
        }
        
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Job newJob)
        {
            // Check if a record with this JMPID already exists.
            var jobsWithMatchingJMPID = _mainDBContext.Jobs.Select(j => j.JMPID == newJob.JMPID);
            if (jobsWithMatchingJMPID.Count() > 0)
            {
                _logger.LogInformation("Unable to create Job, JMPID already in use: {JMPID}", newJob.JMPID);
                return NotFound("JMPID already exists");
            }

            newJob.ID = new Guid();
            
            await _mainDBContext.Jobs.AddAsync(newJob);
            await _mainDBContext.SaveChangesAsync();

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
    }
}
