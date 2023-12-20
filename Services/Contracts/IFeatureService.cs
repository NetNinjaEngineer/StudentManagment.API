using StudentManagement.API.Entities;
using StudentManagement.API.Helpers;

namespace StudentManagement.API.Services.Contracts;

public interface IFeatureService
{
    Task<IEnumerable<ShowStudentsWithCoursesRegisteredModel>>
        GetStudentsWithCoursesRegistered();

    Task<IEnumerable<CoursesWithDepartmentsAndPreRequestsModel>>
       GetCoursesWithDepartmentsAndPreRequests();

    Task<IEnumerable<CoursesWithPreRequestsModel>>
        GetCoursesWithPreRequests();

    Task<(string, string)> AssignCourseToStudent(int courseId, int studentId);

    Task<bool> CheckValidIDS(int courseId, int studentId);

    Task<bool> CheckPreRequestCourse(int courseId, int studentId);

    Task<bool> CheckCourseHaveBeenEnrolled(int courseId, int studentId);

    Task<IEnumerable<CoursesWithDepartmentsModel>> GetCoursesWithDepartments();

    Task<decimal> CalculateTotalGPA(int studentId);

    Task<Enrollment> UpdateEnrollment(Enrollment enrollment);

    Task<Enrollment> GetEnrollmentById(int studentId, int courseId);

    Task<(string, IEnumerable<string>)> SuggestCoursesDependOnDepartments(int studentId);
}
