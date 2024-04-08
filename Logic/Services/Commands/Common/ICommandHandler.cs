using CSharpFunctionalExtensions;

namespace Logic.Services.Commands.Common;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public Result Handle(TCommand command);
}