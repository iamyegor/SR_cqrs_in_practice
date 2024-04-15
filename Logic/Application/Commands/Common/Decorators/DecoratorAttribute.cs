namespace Logic.Application.Commands.Common.Decorators;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public abstract class DecoratorAttribute : Attribute
{
    public abstract Type DecoratorType { get; }
}
