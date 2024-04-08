using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Services.Commands.Common;
using Logic.Students;

namespace Logic.Services.Commands.Transfer;

public class TransferCommandHandler : ICommandHandler<TransferCommand>
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public TransferCommandHandler(
        StudentRepository studentRepository,
        CourseRepository courseRepository
    )
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public Result Handle(TransferCommand command)
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

        Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
        if (enrollment == null)
        {
            return Result.Failure("User doesn't have this enrollment");
        }

        enrollment.Update(course, grade);

        _studentRepository.Save(student);

        return Result.Success();
    }
}
