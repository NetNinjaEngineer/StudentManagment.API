using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Dtos;
using StudentManagement.API.Services.Contracts;

namespace StudentManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FeaturesController(IFeatureService featureService, IMapper mapper) : ControllerBase
{
    private readonly IFeatureService _featureService = featureService;
    private readonly IMapper _mapper = mapper;

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
        var takenPreCourse = await _featureService.CheckPreRequestCourse(courseId, studentId);

        if (takenPreCourse == false)
            return BadRequest($"You must Take PreRequest Course");

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
        var (student, suggestedCourses) = await _featureService
             .SuggestCoursesDependOnDepartments(studentId);

        return Ok($"Courses Suggested For {student}: {String.Join(", ", suggestedCourses)}");
    }
}
