using Api.DTOs;
using Logic.DAL;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students/{studentId}/enrollments")]
public class EnrollmentsController : Controller
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public EnrollmentsController(
        StudentRepository studentRepository,
        CourseRepository courseRepository
    )
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    [HttpPost]
    public IActionResult Enroll(int studentId, StudentForEnrollmentDto studentForEnrollmentDto)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student == null)
        {
            return Error($"No student found for Id {studentId}");
        }

        Course? course = _courseRepository.GetByName(studentForEnrollmentDto.CourseName);
        if (course == null)
        {
            return Error($"No course with name {studentForEnrollmentDto.CourseName}");
        }

        bool gradeParsed = Enum.TryParse(studentForEnrollmentDto.CourseGrade, out Grade grade);
        if (!gradeParsed)
        {
            return Error($"The provided grade {studentForEnrollmentDto.CourseGrade} is incorrect");
        }

        student.Enroll(course, grade);

        _studentRepository.Save(student);

        return Ok();
    }

    [HttpPut("{enrollmentNumber}")]
    public IActionResult Transfer(
        int studentId,
        int enrollmentNumber,
        StudentForTransferDto studentForTransferDto
    )
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student == null)
        {
            return Error($"No student found for Id {studentId}");
        }

        Course? course = _courseRepository.GetByName(studentForTransferDto.CourseName);
        if (course == null)
        {
            return Error($"No course with name {studentForTransferDto.CourseName}");
        }

        bool gradeParsed = Enum.TryParse(studentForTransferDto.CourseGrade, out Grade grade);
        if (!gradeParsed)
        {
            return Error($"The provided grade {studentForTransferDto.CourseGrade} is incorrect");
        }

        Enrollment? enrollment = student.GetEnrollment(enrollmentNumber);
        if (enrollment == null)
        {
            return Error("User doesn't have this enrollment");
        }

        enrollment.Update(course, grade);

        _studentRepository.Save(student);

        return Ok();
    }

    [HttpDelete("{enrollmentNumber}")]
    public IActionResult Disenroll(
        int studentId,
        int enrollmentNumber,
        StudentForDisenrollmentDto studentForDisenrollmentDto
    )
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student == null)
        {
            return Error($"No student found for Id {studentId}");
        }

        Enrollment? enrollment = student.GetEnrollment(enrollmentNumber);
        if (enrollment == null)
        {
            return Error("User doesn't have this enrollment");
        }

        student.Disenroll(enrollment, studentForDisenrollmentDto.Comment);

        _studentRepository.Save(student);

        return Ok();
    }
}
