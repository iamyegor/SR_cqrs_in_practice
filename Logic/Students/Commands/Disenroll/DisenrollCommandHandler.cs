using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Disenroll;

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
            return Result.Failure($"No student found for Id {command.StudentId}");
        }

        Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
        if (enrollment == null)
        {
            return Result.Failure("User doesn't have this enrollment");
        }

        student.Disenroll(enrollment, command.Comment);

        _studentRepository.Save(student);

        return Result.Success();
    }
}
