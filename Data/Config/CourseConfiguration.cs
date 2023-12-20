using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Entities;

namespace StudentManagement.API.Data.Config;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.CourseId);
        builder.Property(c => c.CourseId)
            .ValueGeneratedNever();

        builder.Property(c => c.CourseName)
            .HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();

        builder.Property(c => c.CourseCode)
            .HasColumnType("VARCHAR").HasMaxLength(15).IsRequired();

        builder.Property(c => c.CreditHours)
            .HasColumnType("INT").IsRequired();

        builder.Property(c => c.CourseMark)
            .HasColumnType("INT").IsRequired(false);

        builder.Property(c => c.PreRequest)
            .HasColumnType("INT").IsRequired(false);

        builder.HasOne(x => x.Department)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.ToTable("Courses");
    }
}
