using FluentResults;
using Logic.Application.Commands.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Application.Utils;

public class Messages
{
    private readonly IServiceProvider _serviceProvider;

    public Messages(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result Dispatch(ICommand command)
    {
        Type handlerInterfaceGenericTypeDefinition = typeof(ICommandHandler<>);
        Type handlerInterface = handlerInterfaceGenericTypeDefinition.MakeGenericType(
            command.GetType()
        );

        dynamic handler = _serviceProvider.GetRequiredService(handlerInterface);

        return handler.Hanlde((dynamic)command);
    }
}
