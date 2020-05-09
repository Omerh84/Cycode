using Cycode.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Cycode.DAL
{
    public class CycodeContext : DbContext
    {
        public CycodeContext()
        {
        }

        public CycodeContext(DbContextOptions<CycodeContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentInCourse> StudentInCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(s => s.EmailAddress).IsUnique();
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<StudentInCourse>().HasIndex(sic => new {sic.CourseId, sic.StudentId}).IsUnique();

            modelBuilder.Entity<Student>().HasMany(s => s.StudentsInCourses).WithOne(sic => sic.Student)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Course>().HasMany(c => c.StudentsInCourses).WithOne(sic => sic.Course)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<StudentInCourse>().HasOne(sic => sic.Grade).WithOne(g => g.StudentInCourse)
                .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Course>().HasMany(c => c.Students).WithMany(s => s.Courses);
            //modelBuilder.Entity<Course>().HasMany(c => c.Students).
        }
    }
}