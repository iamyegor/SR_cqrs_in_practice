using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Unregister;

public class UnregisterCommandHandler : ICommandHandler<UnregisterCommand>
{
    private readonly StudentRepository _studentRepository;

    public UnregisterCommandHandler(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Result Handle(UnregisterCommand command)
    {
        Student? student = _studentRepository.GetById(command.Id);
        if (student == null)
        {
            return Result.Failure($"No student found for Id {command.Id}");
        }

        _studentRepository.Delete(student);

        return Result.Success();
    }
}
