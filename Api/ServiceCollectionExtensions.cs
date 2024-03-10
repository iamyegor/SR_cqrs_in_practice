using System.Reflection;
using Logic.Students.Commands.Common;
using Logic.Students.Queries.Common;

namespace Api;

public static class ServiceCollectionExtensions
{
    public static void AddCqrsHandlersFromAssemblyContaining<T>(this IServiceCollection services)
    {
        Assembly? assembly = typeof(T).Assembly;
        if (assembly == null)
        {
            throw new Exception($"No assembly was found for {typeof(T).Name}");
        }

        RegisterHandlers(services, typeof(ICommandHandler<>), assembly);
        RegisterHandlers(services, typeof(IQueryHandler<,>), assembly);
    }

    private static void RegisterHandlers(
        IServiceCollection services,
        Type handlerInterface,
        Assembly assembly
    )
    {
        Type[] handlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            )
            .ToArray();

        foreach (var handlerType in handlerTypes)
        {
            Type interfaceType = handlerType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);

            services.AddTransient(interfaceType, handlerType);
        }
    }
}
