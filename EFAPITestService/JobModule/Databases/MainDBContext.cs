using EFAPITestService.JobModule.Databases.Model;
using Microsoft.EntityFrameworkCore;

namespace EFAPITestService.JobModule.Databases
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options)
        { }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set Job's Primary Key
            modelBuilder.Entity<Job>().HasKey(j => j.JMPID);
        }
    }
}
