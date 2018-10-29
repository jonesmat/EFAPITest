using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAPITestService.JobModule.Databases;
using EFAPITestService.JobModule.Managers;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EFAPITest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var hangfireDBConnection = Configuration.GetConnectionString("HangfireDB");
            services.AddHangfire(x => x.UseSqlServerStorage(hangfireDBConnection));

            var mainDBConnection = Configuration.GetConnectionString("MainDB");
            services.AddDbContext<MainDBContext> (options => options.UseSqlServer(mainDBConnection));

            var oldDBConnection = Configuration.GetConnectionString("OldDB");
            services.AddDbContext<OldDBContext>(options => options.UseSqlServer(oldDBConnection));

            services.AddScoped<JobMgr>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMvc();

            // We want to keep sync setup in the Sync Manager, 
            // but want to ensure it's scheduling code is called as soon as the app starts up
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var syncManager = scope.ServiceProvider.GetRequiredService<JobMgr>();
                JobMgr.ScheduleTasks();
            }

        }
    }
}
