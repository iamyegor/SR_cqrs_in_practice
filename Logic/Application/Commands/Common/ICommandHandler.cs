using FluentResults;

namespace Logic.Application.Commands.Common;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public Result Handle(TCommand command);
}
