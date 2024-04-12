namespace Logic.Application.Commands.Common.Decorators;

public class DatabaseRetryDecorator
{
    private bool IsDatabaseException(Exception exception)
    {
        string? message = exception.InnerException?.Message;
        if (message == null)
        {
            return false;
        }

        return message.Contains("The connection is broken and recovery is not possible")
            || message.Contains("error occurred while establishing a connection");
    }
}
