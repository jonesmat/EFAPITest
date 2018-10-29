using EFAPITest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAPITest.Databases
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
