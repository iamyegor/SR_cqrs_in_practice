using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.DAL.Repositories;
using Logic.Services.Commands.Common;
using Logic.Students;

namespace Logic.Services.Commands.Unregister;

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
