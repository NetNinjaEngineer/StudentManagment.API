using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Entities;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        protected ApplicationDbContext() { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<CoursesWithPreRequestsModel> CoursesWithPreRequests { get; set; }
        public DbSet<ShowStudentsWithCoursesRegisteredModel> StudentsWithCoursesRegistered { get; set; }
        public DbSet<CoursesWithDepartmentsAndPreRequestsModel> CoursesWithDepartmentsAndPreRequests { get; set; }
        public DbSet<CoursesWithDepartmentsModel> CoursesWithDepartments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
