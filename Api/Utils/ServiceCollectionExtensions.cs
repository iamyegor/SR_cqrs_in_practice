using System.Reflection;
using Logic.Application.Commands.Common;
using Logic.Application.Commands.Common.Decorators;
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
            RegisterHandler(handlerType, services);
        }
    }

    private static void RegisterHandler(Type handlerType, IServiceCollection services)
    {
        IEnumerable<Type> decorators = handlerType
            .GetCustomAttributes(false)
            .Where(attr => attr is DecoratorAttribute)
            .Select(attr => ((DecoratorAttribute)attr).DecoratorType);

        IEnumerable<Type> pipeline = decorators.Concat([handlerType]).Reverse();

        Type handlerInterface = handlerType.GetInterfaces().Single(IsHandlerInterface);
        Func<IServiceProvider, object> factory = BuildPipeline(
            pipeline,
            handlerInterface.GetGenericArguments()
        );

        services.AddTransient(handlerInterface, factory);
    }

    private static Func<IServiceProvider, object> BuildPipeline(
        IEnumerable<Type> pipeline,
        Type[] decoratorGenericArguments
    )
    {
        List<ConstructorInfo> constructors = [];
        foreach (var decoratorOrHandler in pipeline)
        {
            // decorators are generic, command handlers themselves are not.
            Type type = decoratorOrHandler.IsGenericTypeDefinition
                ? decoratorOrHandler.MakeGenericType(decoratorGenericArguments)
                : decoratorOrHandler;

            constructors.Add(type.GetConstructors().Single());
        }

        object Factory(IServiceProvider provider)
        {
            object currentHandlerOrDecorator = new object();

            foreach (var constructor in constructors)
            {
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                object[] parametersValues = GetValuesFor(
                    constructorParameters,
                    currentHandlerOrDecorator,
                    provider
                );

                currentHandlerOrDecorator = constructor.Invoke(parametersValues);
            }

            return currentHandlerOrDecorator;
        }

        return Factory;
    }

    private static object[] GetValuesFor(
        ParameterInfo[] constructorParameters,
        object previouslyCreatedHandlerOrDecorator,
        IServiceProvider provider
    )
    {
        List<object> values = [];
        foreach (var parameterType in constructorParameters.Select(p => p.ParameterType))
        {
            values.Add(
                IsHandlerInterface(parameterType)
                    ? previouslyCreatedHandlerOrDecorator
                    : provider.GetRequiredService(parameterType)
            );
        }

        return values.ToArray();
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
