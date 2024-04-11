using FluentResults;
using Logic.Application.Commands.Common;
using Logic.Application.Queries.Common;
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

    public TResult Dispatch<TResult>(IQuery<TResult> query)
    {
        Type handlerInterfaceGenericTypeDefinition = typeof(IQueryHandler<,>);
        Type handlerInterface = handlerInterfaceGenericTypeDefinition.MakeGenericType(
            query.GetType(),
            typeof(TResult)
        );

        dynamic handler = _serviceProvider.GetRequiredService(handlerInterface);
        TResult result = handler.Handle((dynamic)query);

        return result;
    }
}
