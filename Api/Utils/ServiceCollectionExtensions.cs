using Logic.Application.Commands.Common;
using Logic.Application.Queries.Common;
using Logic.Interfaces;

namespace Api.Utils;

public static class ServiceCollectionExtensions
{
    public static void AddCqrsHandlers(this IServiceCollection services)
    {
        IEnumerable<Type> handlerTypes = typeof(ILogicAssembly)
            .Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(IsHandlerInterface));

        foreach (var handlerType in handlerTypes)
        {
            Type handlerInterface = handlerType.GetInterfaces().Single(IsHandlerInterface);
            services.AddTransient(handlerInterface, handlerType);
        }
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        return type.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
            || type.GetGenericTypeDefinition() == typeof(IQueryHandler<,>);
    }
}
