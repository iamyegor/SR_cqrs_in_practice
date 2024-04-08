using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Services.Commands.Common;
using Logic.Services.Commands.Common.Decorators.AuditLogging;
using Logic.Services.Commands.Common.Decorators.DatabaseRetry;
using Logic.Students;
using Logic.Utils;

namespace Logic.Services.Commands.EditPersonalInfo;

[AuditLogging]
[DatabaseRetry]
public class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
{
    private readonly StudentRepository _studentRepository;
    private readonly ExceptionIncrementor _exceptionIncrementor;

    public EditPersonalInfoCommandHandler(
        StudentRepository studentRepository,
        ExceptionIncrementor exceptionIncrementor
    )
    {
        ArgumentNullException.ThrowIfNull(studentRepository);

        _studentRepository = studentRepository;
        _exceptionIncrementor = exceptionIncrementor;
    }

    public Result Handle(EditPersonalInfoCommand command)
    {
        if (_exceptionIncrementor.Value < 1)
        {
            _exceptionIncrementor.Increment();
            throw new Exception("The connection is broken and recovery is not possible");
        }

        Student? student = _studentRepository.GetById(command.Id);
        if (student == null)
        {
            return Result.Failure($"No student found for Id {command.Id}");
        }

        student.Name = command.Name;
        student.Email = command.Email;

        _studentRepository.Save(student);

        return Result.Success();
    }
}
