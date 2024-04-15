namespace Logic.Application.Commands.Common.Decorators.AuditLogging;

public class AuditLoggingAttribute : DecoratorAttribute
{
    public override Type DecoratorType => typeof(AuditLoggingDecorator<>);
}
