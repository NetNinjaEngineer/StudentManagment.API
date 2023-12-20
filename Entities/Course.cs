namespace StudentManagement.API.Entities;

public class Course
{
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? CourseCode { get; set; }
    public int CreditHours { get; set; }
    public int? PreRequest { get; set; }
    public int? CourseMark { get; set; }

    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

