namespace StudentManagement.API.Entities;

public class Department
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? DepartmentAbbreviation { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

