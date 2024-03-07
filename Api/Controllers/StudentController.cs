using Api.DTOs;
using Logic.DAL.Repositories;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    [HttpGet]
    public IActionResult GetList(string? enrolled, int? number)
    {
        // IReadOnlyList<Student> studentsFromDb = _studentRepository.GetList(enrolled, number);
        //
        // return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentsFromDb));
        return Ok();
    }

    [HttpPost]
    public IActionResult Register(StudentForRegistrationDto studentDto)
    {
        var student = new Student(studentDto.Name, studentDto.Email);

        if (
            !string.IsNullOrEmpty(studentDto.Course1)
            && !string.IsNullOrEmpty(studentDto.Course1Grade)
        )
        {
            Course? course = _courseRepository.GetByName(studentDto.Course1);
            if (course == null)
            {
                return Error($"Course with name {studentDto.Course1} doesn't exist");
            }

            if (!Enum.TryParse(studentDto.Course1Grade, out Grade grade))
            {
                return Error($"Invalid grade value: {studentDto.Course1Grade}");
            }
            student.Enroll(course, grade);
        }

        if (
            !string.IsNullOrEmpty(studentDto.Course2)
            && !string.IsNullOrEmpty(studentDto.Course2Grade)
        )
        {
            Course? course = _courseRepository.GetByName(studentDto.Course2);
            if (course == null)
            {
                return Error($"Course with name {studentDto.Course2} doesn't exist");
            }

            if (!Enum.TryParse(studentDto.Course2Grade, out Grade grade))
            {
                return Error($"Invalid grade value: {studentDto.Course2Grade}");
            }
            student.Enroll(course, grade);
        }

        _studentRepository.Add(student);

        return CreatedAtAction(nameof(Register), new { id = student.Id }, student);
    }

    [HttpDelete("{id}")]
    public IActionResult Unregister(int id)
    {
        Student? student = _studentRepository.GetById(id);
        if (student == null)
        {
            return Error($"No student found for Id {id}");
        }

        _studentRepository.Delete(student);

        return Ok();
    }

    [HttpPut("{studentId}")]
    public IActionResult EditPersonalInfo(
        int studentId,
        StudentForEditPersonalInfoDto studentForEditPersonalInfoDto
    )
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student == null)
        {
            return Error($"No student found for Id {studentId}");
        }

        student.Name = studentForEditPersonalInfoDto.Name;
        student.Email = studentForEditPersonalInfoDto.Email;

        _studentRepository.Save(student);

        return Ok();
    }
}
