using FluentResults;
using Logic.Application.Commands.Common;
using Logic.DAL.Repositories;
using Logic.Students;

namespace Logic.Application.Commands.Unregister;

public class UnregisterCommandHandler : ICommandHandler<UnregisterCommand>
{
    private readonly StudentRepository _studentRepository;

    public UnregisterCommandHandler(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Result Handle(UnregisterCommand command)
    {
        Student? student = _studentRepository.GetById(command.StudentId);
        if (student == null)
        {
            return Result.Fail($"No student found for Id {command.StudentId}");
        }

        _studentRepository.Delete(student);

        return Result.Ok();
    }
}
