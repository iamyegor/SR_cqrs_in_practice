namespace Logic.Application.Commands.Common.Decorators.DatabaseRetry;

public class DatabaseRetryAttribute : DecoratorAttribute
{
    public override Type DecoratorType => typeof(DatabaseRetryDecorator<>);
}
