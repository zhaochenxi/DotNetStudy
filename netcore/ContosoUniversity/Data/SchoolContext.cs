using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<ContosoUniversity.Models.Student> Student { get; set; }
        public DbSet<ContosoUniversity.Models.Enrollment> Enrollment { get; set; }
        public DbSet<ContosoUniversity.Models.Course> Course { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContosoUniversity.Models.Course>().ToTable("Course");
            modelBuilder.Entity<ContosoUniversity.Models.Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<ContosoUniversity.Models.Student>().ToTable("Student");
        }
    }
}
