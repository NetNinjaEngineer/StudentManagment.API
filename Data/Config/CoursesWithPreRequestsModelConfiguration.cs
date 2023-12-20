using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Data.Config;

public class CoursesWithPreRequestsModelConfiguration :
    IEntityTypeConfiguration<CoursesWithPreRequestsModel>
{
    public void Configure(EntityTypeBuilder<CoursesWithPreRequestsModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("CoursesWithPreRequests");
    }
}

