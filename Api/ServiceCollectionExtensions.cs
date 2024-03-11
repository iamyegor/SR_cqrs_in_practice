using System.Reflection;
using System.Runtime.CompilerServices;
using Logic.Students.Commands.Common;
using Logic.Students.Commands.Common.Decorators.AuditLogging;
using Logic.Students.Commands.Common.Decorators.DatabaseRetry;
using Logic.Students.Queries.Common;

namespace Api;

public static class ServiceCollectionExtensions
{
    public static void AddCqrsHandlers(this IServiceCollection services)
    {
        List<Type> handlerTypes = typeof(ICommand)
            .Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(IsHandlerInterface))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            AddHandler(services, handlerType);
        }
    }

    private static void AddHandler(IServiceCollection services, Type handlerType)
    {
        List<Type> decorators = handlerType
            .GetCustomAttributes(false)
            .Where(attr => attr is not NullableContextAttribute && attr is not NullableAttribute)
            .Select(GetDecoratorFromAttribute)
            .ToList();

        List<Type> pipeline = decorators.Concat([handlerType]).Reverse().ToList();

        Type handlerInterface = handlerType.GetInterfaces().Single(IsHandlerInterface);
        Func<IServiceProvider, object> factory = BuildPipeline(pipeline, handlerInterface);

        services.AddTransient(handlerInterface, factory);
    }

    private static Func<IServiceProvider, object> BuildPipeline(
        List<Type> pipeline,
        Type handlerInterface
    )
    {
        List<ConstructorInfo> constructors = [];
        foreach (var handlerOrDecorator in pipeline)
        {
            Type type = handlerOrDecorator.IsGenericType
                ? handlerOrDecorator.MakeGenericType(handlerInterface.GenericTypeArguments)
                : handlerOrDecorator;

            constructors.Add(type.GetConstructors().Single());
        }

        object Factory(IServiceProvider serviceProvider)
        {
            object currentHandlerOrDecorator = new object();

            foreach (var constructor in constructors)
            {
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                object[] valuesOfParameters = GetValuesOfParameters(
                    constructorParameters,
                    currentHandlerOrDecorator,
                    serviceProvider
                );

                currentHandlerOrDecorator = constructor.Invoke(valuesOfParameters);
            }

            return currentHandlerOrDecorator;
        }

        return Factory;
    }

    private static object[] GetValuesOfParameters(
        ParameterInfo[] constructorParameters,
        object previouslyCreatedHandlerOrDecorator,
        IServiceProvider serviceProvider
    )
    {
        object[] values = new object[constructorParameters.Length];

        for (int i = 0; i < constructorParameters.Length; i++)
        {
            Type parameterType = constructorParameters[i].ParameterType;

            if (IsHandlerInterface(parameterType))
            {
                values[i] = previouslyCreatedHandlerOrDecorator;
            }
            else
            {
                values[i] = serviceProvider.GetRequiredService(parameterType);
            }
        }

        return values;
    }

    private static Type GetDecoratorFromAttribute(object attribute)
    {
        if (attribute.GetType() == typeof(AuditLoggingAttribute))
        {
            return typeof(AuditLoggingDecorator<>);
        }
        else if (attribute.GetType() == typeof(DatabaseRetryAttribute))
        {
            return typeof(DataBaseRetryDecorator<>);
        }

        throw new Exception(
            $"Attribute of type {attribute.GetType().Name} doesn't match any decorator"
        );
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (type.IsGenericType)
        {
            Type genericType = type.GetGenericTypeDefinition();
            return genericType == typeof(ICommandHandler<>)
                || genericType == typeof(IQueryHandler<,>);
        }

        return false;
    }
}
