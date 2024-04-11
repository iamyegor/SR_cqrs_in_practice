using FluentResults;
using Logic.Application.Commands.Common;
using Logic.DAL.Repositories;
using Logic.Students;

namespace Logic.Application.Commands.Disenroll;

public class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
{
    private readonly StudentRepository _studentRepository;

    public DisenrollCommandHandler(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Result Handle(DisenrollCommand command)
    {
        Student? student = _studentRepository.GetById(command.StudentId);
        if (student == null)
        {
            return Result.Fail($"No student found for Id {command.StudentId}");
        }

        Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
        if (enrollment == null)
        {
            return Result.Fail("User doesn't have this enrollment");
        }

        student.Disenroll(enrollment, command.Comment);

        _studentRepository.Save(student);

        return Result.Ok();
    }
}
