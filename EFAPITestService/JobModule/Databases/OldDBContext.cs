using EFAPITestService.JobModule.Databases.Model;
using Microsoft.EntityFrameworkCore;

namespace EFAPITestService.JobModule.Databases
{
    public class OldDBContext : DbContext
    {
        public OldDBContext(DbContextOptions<OldDBContext> options) : base(options)
        { }

        public DbSet<OldJob> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set OldJob's Primary Key
            modelBuilder.Entity<OldJob>().HasKey(j => j.JMPID);
        }
    }
}
