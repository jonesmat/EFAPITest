using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAPITest.Databases;
using EFAPITest.Managers;
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

            app.UseMvc();
        }
    }
}
