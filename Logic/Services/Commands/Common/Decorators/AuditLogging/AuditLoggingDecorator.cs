using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace Logic.Services.Commands.Common.Decorators.AuditLogging;

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
