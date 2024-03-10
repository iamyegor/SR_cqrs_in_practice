using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;

    public RegisterCommandHandler(
        CourseRepository courseRepository,
        StudentRepository studentRepository
    )
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public Result Handle(RegisterCommand command)
    {
        var student = new Student(command.Name, command.Email);

        if (command is { Course1: not null, Course1Grade: not null })
        {
            Course? course = _courseRepository.GetByName(command.Course1);
            if (course == null)
            {
                return Result.Failure($"Course with name {command.Course1} doesn't exist");
            }

            student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
        }

        if (command is { Course2: not null, Course2Grade: not null })
        {
            Course? course = _courseRepository.GetByName(command.Course2);
            if (course == null)
            {
                return Result.Failure($"Course with name {command.Course2} doesn't exist");
            }

            student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
        }

        _studentRepository.Add(student);

        return Result.Success();
    }
}
