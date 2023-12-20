using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Data.Config;

public class CoursesWithDepartmentsModelConfiguration :
    IEntityTypeConfiguration<CoursesWithDepartmentsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithDepartmentsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowCoursesWithDepartments");
    }
}
