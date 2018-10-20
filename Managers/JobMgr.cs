using EFAPITest.Databases;
using EFAPITest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAPITest.Managers
{
    public class JobMgr
    {
        ILogger _logger;
        MainDBContext _mainDBContext;
        OldDBContext _oldDBContext;

        public JobMgr(ILogger<JobMgr> logger, MainDBContext mainDBContext,
                        OldDBContext oldDBContext)
        {
            _logger = logger;
            _mainDBContext = mainDBContext;
            _oldDBContext = oldDBContext;
        }

        public async Task<List<Job>> GetJobs()
        {
            List<Job> jobs = await _mainDBContext.Jobs.ToListAsync();
            List<Job> convertedOldJobs = await _oldDBContext.Jobs.Select(oj => ConvertOldJob(oj)).ToListAsync();

            jobs.AddRange(convertedOldJobs);

            return jobs;
        }

        public async Task CreateJob(Job job)
        {
            await _mainDBContext.Jobs.AddAsync(job);
            await _mainDBContext.SaveChangesAsync();
        }

        private Job ConvertOldJob(OldJob oldJob)
        {
            Job job = new Job();
            job.JMPID = oldJob.JMPID;
            job.CrewName = oldJob.CrewName;
            return job;
        }
    }
}
