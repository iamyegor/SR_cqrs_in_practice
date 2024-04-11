using FluentResults;
using Logic.Application.Commands.Common;
using Logic.DAL;
using Logic.DAL.Repositories;
using Logic.Students;

namespace Logic.Application.Commands.Transfer;

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
            return Result.Fail($"No student found for Id {command.StudentId}");
        }

        Course? course = _courseRepository.GetByName(command.Course);
        if (course == null)
        {
            return Result.Fail($"No course with name {command.Course}");
        }

        bool gradeParsed = Enum.TryParse(command.Grade, out Grade grade);
        if (!gradeParsed)
        {
            return Result.Fail($"The provided grade {command.Grade} is incorrect");
        }

        Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
        if (enrollment == null)
        {
            return Result.Fail("User doesn't have this enrollment");
        }

        enrollment.Update(course, grade);

        _studentRepository.Save(student);

        return Result.Ok();
    }
}
