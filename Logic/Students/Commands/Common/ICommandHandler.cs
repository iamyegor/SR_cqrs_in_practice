using CSharpFunctionalExtensions;

namespace Logic.Students.Commands.Common;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public Result Handle(TCommand command);
}