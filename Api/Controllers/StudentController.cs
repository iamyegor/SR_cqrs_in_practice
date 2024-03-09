using Api.DTOs;
using AutoMapper;
using Logic.DAL;
using Logic.Students;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public StudentController(
        StudentRepository studentRepository,
        CourseRepository courseRepository,
        IMapper mapper
    )
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetList(string? enrolled, int? number)
    {
        IReadOnlyList<Student> studentsFromDb = _studentRepository.GetList(enrolled, number);

        return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentsFromDb));
    }

    [HttpPost]
    public IActionResult Register([FromBody] StudentForCreationDto studentDto)
    {
        var student = new Student(studentDto.Name, studentDto.Email);

        if (studentDto is { Course1: not null, Course1Grade: not null })
        {
            Course? course = _courseRepository.GetByName(studentDto.Course1);
            if (course == null)
            {
                return Error($"Course with name {studentDto.Course1} doesn't exist");
            }

            student.Enroll(course, Enum.Parse<Grade>(studentDto.Course1Grade));
        }

        if (studentDto is { Course2: not null, Course2Grade: not null })
        {
            Course? course = _courseRepository.GetByName(studentDto.Course2);
            if (course == null)
            {
                return Error($"Course with name {studentDto.Course2} doesn't exist");
            }

            student.Enroll(course, Enum.Parse<Grade>(studentDto.Course2Grade));
        }

        _studentRepository.Add(student);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Unregister(long id)
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
    public IActionResult EditPersonalInfo(int studentId, StudentForUpdateDto studentForUpdateDto)
    {
        Student? student = _studentRepository.GetById(studentId);
        if (student == null)
        {
            return Error($"No student found for Id {studentId}");
        }

        _mapper.Map(studentForUpdateDto, student);

        _studentRepository.Save(student);

        return Ok();
    }
}
