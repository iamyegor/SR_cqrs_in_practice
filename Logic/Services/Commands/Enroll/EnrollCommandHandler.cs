using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Services.Commands.Common;
using Logic.Students;

namespace Logic.Services.Commands.Enroll;

public class EnrollCommandHandler : ICommandHandler<EnrollCommand>
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public EnrollCommandHandler(
        StudentRepository studentRepository,
        CourseRepository courseRepository
    )
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public Result Handle(EnrollCommand command)
    {
        Student? student = _studentRepository.GetById(command.StudentId);
        if (student == null)
        {
            return Result.Failure($"No student found for Id {command.StudentId}");
        }

        Course? course = _courseRepository.GetByName(command.Course);
        if (course == null)
        {
            return Result.Failure($"No course with name {command.Course}");
        }

        bool gradeParsed = Enum.TryParse(command.Grade, out Grade grade);
        if (!gradeParsed)
        {
            return Result.Failure($"The provided grade {command.Grade} is incorrect");
        }

        student.Enroll(course, grade);

        _studentRepository.Save(student);

        return Result.Success();
    }
}
