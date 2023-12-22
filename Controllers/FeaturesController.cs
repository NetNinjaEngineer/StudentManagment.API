using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Data;
using StudentManagement.API.Dtos;
using StudentManagement.API.Entities;
using StudentManagement.API.Services.Contracts;

namespace StudentManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FeaturesController(IFeatureService featureService,
    IMapper mapper, ApplicationDbContext context) : ControllerBase
{
    private readonly IFeatureService _featureService = featureService;
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;

    [Route("GetStudentsWithCoursesRegistered")]
    [HttpGet]
    public async Task<IActionResult> GetStudentsWithCoursesRegistered()
    {
        var studentsWithCoursesRegistered = await _featureService.GetStudentsWithCoursesRegistered();
        if (studentsWithCoursesRegistered == null)
            return NotFound("There is not students with courses registered !!!");

        return Ok(studentsWithCoursesRegistered);
    }

    [Route("GetCoursesWithDepartments")]
    [HttpGet]
    public async Task<IActionResult> GetCoursesWithDepartments()
    {
        var coursesWithDepartments = await _featureService.GetCoursesWithDepartments();
        if (coursesWithDepartments == null)
            return NotFound("There is not courses with departments !!!");

        return Ok(coursesWithDepartments);
    }


    [Route("GetCoursesWithDepartmentsAndPreRequests")]
    [HttpGet]
    public async Task<IActionResult> GetCoursesWithDepartmentsAndPreRequests()
    {
        var coursesWithDepartmentsAndPreRequests = await _featureService.GetCoursesWithDepartmentsAndPreRequests();
        if (coursesWithDepartmentsAndPreRequests == null)
            return NotFound("There is not Departments with courses having PreRequests !!!");

        return Ok(coursesWithDepartmentsAndPreRequests);
    }

    [Route("GetCoursesWithPreRequests")]
    [HttpGet]
    public async Task<IActionResult> GetCoursesWithPreRequests()
    {
        var coursesWithPreRequests = await _featureService.GetCoursesWithPreRequests();
        if (coursesWithPreRequests == null)
            return NotFound("There is not courses with PreRequests !!!");

        return Ok(coursesWithPreRequests);
    }

    [HttpPost("EnrollCourseToStudent")] // course 15 student 9
    public async Task<IActionResult> EnrollCourseToStudent(int courseId, int studentId)
    {
        // case1 not valid ids
        var validIDS = await _featureService
            .CheckValidIDS(courseId, studentId);

        if (!validIDS)
            return BadRequest("Not valid course or student, " +
                "can not enroll the course, try again !!!");

        // case2 student does not register course if already register PreRequest course
        var (taken, course) = await _featureService.CheckPreRequestCourse(courseId, studentId);

        if (taken == false)
            return BadRequest($"You must Take {course} Course");

        // case3 student already registered course
        var enrolled = await _featureService.CheckCourseHaveBeenEnrolled(courseId, studentId);

        if (enrolled)
            return BadRequest("You Already Enrolled This course");

        var (courseName, studentName) = await _featureService.AssignCourseToStudent(courseId, studentId);

        return Ok($"{courseName} Assigned To {studentName} successfully");
    }

    [HttpGet("CalculateTotalGPAFor")]
    public async Task<IActionResult> CalculateTotalGPAFor(int studentId)
    {
        var totalGPA = await _featureService.CalculateTotalGPA(studentId);

        if (totalGPA == 0.0m)
            return BadRequest("GPA is Un available");

        return Ok(totalGPA);
    }

    [HttpPut("{studentId}/{courseId}")]
    public async Task<IActionResult> UpdateEnrollementAsync(int studentId, int courseId,
         [FromBody] EnrollementDTO dto)
    {
        var existingEnrollement = await _featureService.GetEnrollmentById(studentId, courseId);
        if (existingEnrollement == null)
            return BadRequest("No enrollement founded !!!");

        _mapper.Map(dto, existingEnrollement);

        var updatedEnrollement = await _featureService.UpdateEnrollment(existingEnrollement);

        return Ok(updatedEnrollement);
    }

    [HttpGet("SuggestCourses")]
    public async Task<IActionResult> SuggestCoursesAsync(int studentId)
    {
        var (message, suggestedCourses) = await _featureService
             .SuggestCoursesDependOnDepartments(studentId);

        return Ok($"{message} {string.Join(", ", suggestedCourses)}");
    }

    [HttpDelete("DeleteStudent/{id}")]
    public async Task<ActionResult<Student>> DeleteAsync(int id)
    {
        var student = await _featureService.GetStudentById(id);
        if (student is not null)
        {
            var deleted = await _featureService.DeleteStudent(id);
            return Ok(deleted);
        }

        return BadRequest($"No student founded with id: {id}");

    }


    [HttpGet("GetEnrolledCourses")]
    public async Task<IActionResult> GetEnrolledCoursesFor(int studentId)
    {
        var enrolledCourses = await _featureService.GetEnrolledCoursesFor(studentId);

        if (!enrolledCourses.Any())
            return NotFound($"There is no enrolled courses founded with id: {studentId}");

        return Ok(enrolledCourses);
    }
}
