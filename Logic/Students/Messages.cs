using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Students;

public class Messages
{
    private readonly IServiceProvider _serviceProvider;

    public Messages(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result Dispatch(ICommand command)
    {
        Type type = typeof(ICommandHandler<>);
        Type[] typeArgs = [command.GetType()];
        Type handlerType = type.MakeGenericType(typeArgs);

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        Result result = handler.Handle((dynamic)command);

        return result;
    }
}
