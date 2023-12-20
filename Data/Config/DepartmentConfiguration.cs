using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Entities;

namespace StudentManagement.API.Data.Config;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.DepartmentId);
        builder.Property(d => d.DepartmentId)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.DepartmentName)
            .HasColumnType("VARCHAR").HasMaxLength(100).IsRequired();

        builder.Property(d => d.DepartmentAbbreviation)
            .HasColumnType("VARCHAR").HasMaxLength(5).IsRequired();

        builder.ToTable("Departments");
    }
}

