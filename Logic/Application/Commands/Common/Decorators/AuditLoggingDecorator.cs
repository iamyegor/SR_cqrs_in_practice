using FluentResults;
using Newtonsoft.Json;

namespace Logic.Application.Commands.Common.Decorators;

public class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public AuditLoggingDecorator(ICommandHandler<TCommand> handler)
    {
        _handler = handler;
    }

    public Result Handle(TCommand command)
    {
        string commandJson = JsonConvert.SerializeObject(command);
        Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

        return _handler.Handle(command);
    }
}
