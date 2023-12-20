using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Entities;

namespace StudentManagement.API.Data.Config;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(c => c.StudentId);
        builder.Property(c => c.StudentId)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.FirstName)
            .HasColumnType("VARCHAR").HasMaxLength(20).IsRequired();

        builder.Property(c => c.LastName)
            .HasColumnType("VARCHAR").HasMaxLength(20).IsRequired();

        builder.Property(c => c.Email)
            .HasColumnType("VARCHAR").HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Phone)
            .HasColumnType("VARCHAR").HasMaxLength(30)
            .IsRequired();

        builder.ToTable("Students");
    }
}
