namespace StudentManagement.API.Entities;

public class Enrollment
{
    public int? StudentMark { get; set; }

    public Student Student { get; set; }
    public int StudentId { get; set; }

    public Course Course { get; set; }
    public int CourseId { get; set; }
}