using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Entities;
using StudentManagement.API.Helpers;
using StudentManagement.API.Services.Contracts;

namespace StudentManagement.API.Services;

public class FeatureService(ApplicationDbContext context) : IFeatureService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<(string, string)> AssignCourseToStudent(int courseId, int studentId)
    {
        var courseName = _context.Courses
            .FirstOrDefault(x => x.CourseId == courseId)?.CourseName;

        var studentName = string.Concat(_context.Students
            .FirstOrDefault(x => x.StudentId == studentId)?.FirstName, ' ', _context.Students
            .FirstOrDefault(x => x.StudentId == studentId)?.FirstName
        );

        await _context.Enrollments
            .AddAsync(new Enrollment() { StudentId = studentId, CourseId = courseId });

        await _context.SaveChangesAsync();

        return ValueTuple.Create(courseName, studentName)!;

    }

    public Task<decimal> CalculateTotalGPA(int studentId)
    {
        decimal totalCreditHours = 0;
        decimal totalGradePoints = 0;

        var query = from e in context.Enrollments
                    join c in context.Courses on e.CourseId equals c.CourseId
                    where e.StudentId == studentId
                    select new
                    {
                        CourseCreditHours = c.CreditHours,
                        Mark = e.StudentMark
                    };

        foreach (var item in query)
        {
            decimal gradePoint = CalculateRatePoint(item.Mark ?? 0);
            totalCreditHours += item.CourseCreditHours;
            totalGradePoints += gradePoint * item.CourseCreditHours;
        }

        decimal gpa = totalCreditHours > 0 ? totalGradePoints / totalCreditHours : 0;
        return Task.FromResult(Math.Round(gpa, 1));
    }

    private decimal CalculateRatePoint(int? studentMark)
    {
        decimal ratePoint = default;
        if (studentMark >= 90 && studentMark <= 100)
            ratePoint = 4.0m;

        else if (studentMark >= 80 && studentMark < 90)
            ratePoint = 3.0m;

        else if (studentMark >= 70 && studentMark < 80)
            ratePoint = 2.0m;

        else if (studentMark >= 60 && studentMark < 70)
            ratePoint = 1.0m;

        else
            ratePoint = 0.0m;

        return ratePoint;

    }

    public async Task<bool> CheckCourseHaveBeenEnrolled(int courseId, int studentId)
    {
        var enrolledCourse = await _context.Enrollments.FirstOrDefaultAsync(x =>
            x.CourseId == courseId && x.StudentId == studentId);

        if (enrolledCourse is not null)
            return true;

        return false;
    }

    public async Task<bool> CheckPreRequestCourse(int courseId, int studentId)
    {
        int? hasPreRequest = null;

        var query = _context.Courses.Where(x => x.PreRequest != null && x.CourseId == courseId)
            .FirstOrDefault()?.PreRequest;

        if (query == null)
            hasPreRequest = 0;
        else
            hasPreRequest = query;

        var courseWithNoPreRequest = (from course in _context.Courses
                                      join coursePre in _context.Courses on
                                     course.CourseId equals coursePre.CourseId
                                      where course.PreRequest == null && coursePre.CourseId == courseId
                                      select course).FirstOrDefault();

        if (courseWithNoPreRequest != null)
            return true;

        var checkTakenCourse = await _context.Enrollments.FirstOrDefaultAsync(x =>
            x.CourseId == hasPreRequest && x.StudentId == studentId);

        if (checkTakenCourse is not null)
            return true;
        else
            return false;

    }

    public async Task<bool> CheckValidIDS(int courseId, int studentId)
    {
        var validStudentId = await _context.Students.AnyAsync(x => x.StudentId == studentId);
        var validCourseId = await _context.Courses.AnyAsync(x => x.CourseId == courseId);

        if (!validCourseId || !validStudentId)
            return false;

        return true;

    }

    public async Task<IEnumerable<CoursesWithDepartmentsModel>> GetCoursesWithDepartments()
    {
        var coursesWithDepartments = await _context.CoursesWithDepartments.ToListAsync();
        return coursesWithDepartments;
    }

    public async Task<IEnumerable<CoursesWithDepartmentsAndPreRequestsModel>> GetCoursesWithDepartmentsAndPreRequests()
    {
        var coursesWithDepartmentsAndPreRequests =
            await _context.CoursesWithDepartmentsAndPreRequests.ToListAsync();

        return coursesWithDepartmentsAndPreRequests;
    }

    public async Task<IEnumerable<CoursesWithPreRequestsModel>> GetCoursesWithPreRequests()
    {
        var coursesWithPreRequests = await _context.CoursesWithPreRequests.ToListAsync();
        return coursesWithPreRequests;
    }

    public async Task<IEnumerable<ShowStudentsWithCoursesRegisteredModel>> GetStudentsWithCoursesRegistered()
    {
        var studentsWithCoursesRegistered =
            await _context.StudentsWithCoursesRegistered.ToListAsync();

        return studentsWithCoursesRegistered;
    }

    public async Task<Enrollment> UpdateEnrollment(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment> GetEnrollmentById(int studentId, int courseId)
    {
        var enrollement = await _context.Enrollments.SingleOrDefaultAsync(
            x => x.StudentId == studentId && x.CourseId == courseId);

        if (enrollement is not null)
            return enrollement;

        return default!;
    }

    public async Task<(string, IEnumerable<string>)> SuggestCoursesDependOnDepartments(int studentId)
    {
        var existStudent = await _context.Students.FirstOrDefaultAsync(
            x => x.StudentId == studentId);

        if (existStudent is not null)
        {
            var totalGPA = await CalculateTotalGPA(studentId);

            if (totalGPA >= 2.0m && totalGPA <= 2.2m)
            {
                var suggestedCourses = GetSuggestedCourses(studentId);

                return ValueTuple.Create($"{String.Concat(
                    existStudent.FirstName, ' ', existStudent.LastName)}", suggestedCourses);
            }
            else
                return ValueTuple.Create("", Enumerable.Empty<string>());
        }

        return ValueTuple.Create($"No student founded with id: {studentId}", Enumerable.Empty<string>());

    }

    private IEnumerable<string> GetSuggestedCourses(int studentId)
    {
        var departmemt = (from course in _context.Courses
                          join department in _context.Departments
                          on course.DepartmentId equals department.DepartmentId
                          join enrollment in _context.Enrollments
                          on course.CourseId equals enrollment.CourseId
                          join student in _context.Students
                          on enrollment.StudentId equals student.StudentId
                          where student.StudentId == studentId
                          select department.DepartmentName).FirstOrDefault();

        var suggestedCourses = from course in _context.Courses
                               join department in _context.Departments
                               on course.DepartmentId equals department.DepartmentId
                               join enrollment in _context.Enrollments
                               on course.CourseId equals enrollment.CourseId
                               join student in _context.Students
                               on enrollment.StudentId equals student.StudentId
                               where course.PreRequest == null && department.DepartmentName!.Equals(departmemt)
                               select course.CourseName;

        return suggestedCourses.AsEnumerable();
    }
}
