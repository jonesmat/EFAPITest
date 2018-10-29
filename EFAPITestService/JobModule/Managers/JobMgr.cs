using EFAPITestService.JobModule.Databases;
using EFAPITestService.JobModule.Databases.Model;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAPITestService.JobModule.Managers
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

        private const string CONVERT_JOBS_TASK = "CONVERT_JOBS_TASK";
        public void ScheduleTasks()
        {
            RecurringJob.AddOrUpdate(CONVERT_JOBS_TASK, ConvertAllOldJobs(JobCancellationToken.Null), Cron.Minutely);


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

        [DisableConcurrentExecution(timeoutInSeconds: 30)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public void ConvertAllOldJobs(IJobCancellationToken cancellationToken)
        {
            long loops = 0;
            int maxLoops = new Random().Next(1000, 10000);
            while (loops != maxLoops)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var jobs = _mainDBContext.Jobs.ToList();

                loops++;
            }
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
