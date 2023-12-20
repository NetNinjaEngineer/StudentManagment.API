using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Data.Config;

public class CoursesWithDepartmentsAndPreRequestsConfiguration :
    IEntityTypeConfiguration<CoursesWithDepartmentsAndPreRequestsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithDepartmentsAndPreRequestsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("ShowCoursesWithDepartmentsAndPreRequests");
    }
}
