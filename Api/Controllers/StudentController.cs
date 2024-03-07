using Api.DTOs;
using AutoMapper;
using Logic.DAL;
using Logic.Students;
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
    public IActionResult Create([FromBody] StudentDto studentDto)
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

        _studentRepository.Save(student);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        Student? student = _studentRepository.GetById(id);
        if (student == null)
        {
            return Error($"No student found for Id {id}");
        }

        _studentRepository.Delete(student);

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] StudentDto studentDto)
    {
        Student? student = _studentRepository.GetById(id);
        if (student == null)
        {
            return Error($"No student found for Id {id}");
        }

        student.Name = studentDto.Name;
        student.Email = studentDto.Email;

        Enrollment? firstEnrollment = student.FirstEnrollment;
        Enrollment? secondEnrollment = student.SecondEnrollment;

        if (HasEnrollmentChanged(studentDto.Course1, studentDto.Course1Grade, firstEnrollment))
        {
            if (string.IsNullOrWhiteSpace(studentDto.Course1)) // Student disenrolls
            {
                if (string.IsNullOrWhiteSpace(studentDto.Course1DisenrollmentComment))
                {
                    return Error("Disenrollment comment is required");
                }

                if (firstEnrollment == null)
                {
                    return Error("Can't disenroll if not enrolled in the course");
                }

                student.RemoveEnrollment(firstEnrollment);
                student.AddDisenrollmentComment(
                    firstEnrollment,
                    studentDto.Course1DisenrollmentComment
                );
            }

            if (string.IsNullOrWhiteSpace(studentDto.Course1Grade))
            {
                return Error("Grade is required");
            }

            Course? course = _courseRepository.GetByName(studentDto.Course1);
            if (course == null)
            {
                return Error("Can't enroll in unexisting course");
            }

            if (firstEnrollment == null)
            {
                // Student enrolls
                student.Enroll(course, Enum.Parse<Grade>(studentDto.Course1Grade));
            }
            else
            {
                // Student transfers
                firstEnrollment.Update(course, Enum.Parse<Grade>(studentDto.Course1Grade));
            }
        }

        if (HasEnrollmentChanged(studentDto.Course2, studentDto.Course2Grade, secondEnrollment))
        {
            if (string.IsNullOrWhiteSpace(studentDto.Course2)) // Student disenrolls
            {
                if (string.IsNullOrWhiteSpace(studentDto.Course2DisenrollmentComment))
                {
                    return Error("Disenrollment comment is required");
                }

                if (secondEnrollment == null)
                {
                    return Error("Can't disenroll if not enrolled in the course");
                }

                student.RemoveEnrollment(secondEnrollment);
                student.AddDisenrollmentComment(
                    secondEnrollment,
                    studentDto.Course2DisenrollmentComment
                );
            }

            if (string.IsNullOrWhiteSpace(studentDto.Course2Grade))
            {
                return Error("Grade is required");
            }

            Course? course = _courseRepository.GetByName(studentDto.Course2);
            if (course == null)
            {
                return Error("Can't enroll in unexisting course");
            }

            if (secondEnrollment == null)
            {
                // Student enrolls
                student.Enroll(course, Enum.Parse<Grade>(studentDto.Course2Grade));
            }
            else
            {
                // Student transfers
                secondEnrollment.Update(course, Enum.Parse<Grade>(studentDto.Course2Grade));
            }
        }

        _studentRepository.Save(student);

        return Ok();
    }

    private bool HasEnrollmentChanged(
        string? newCourseName,
        string? newGrade,
        Enrollment? currentEnrollment
    )
    {
        if (string.IsNullOrWhiteSpace(newCourseName) && currentEnrollment == null)
            return false;

        if (string.IsNullOrWhiteSpace(newCourseName) || currentEnrollment == null)
            return true;

        return newCourseName != currentEnrollment.Course.Name
            || newGrade != currentEnrollment.Grade.ToString();
    }
}
