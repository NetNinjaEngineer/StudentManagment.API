using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Data.Config;

public class StudentsWithCoursesRegisteredConfiguration :
    IEntityTypeConfiguration<ShowStudentsWithCoursesRegisteredModel>
{
    public void Configure(EntityTypeBuilder<ShowStudentsWithCoursesRegisteredModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowStudentsWithCoursesRegistered");
    }
}

