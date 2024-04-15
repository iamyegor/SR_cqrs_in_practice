using FluentResults;

namespace Logic.Application.Commands.Common.Decorators.DatabaseRetry;

public class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly int _databaseRetries;

    public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, int databaseRetries)
    {
        _handler = handler;
        _databaseRetries = databaseRetries;
    }

    public Result Handle(TCommand command)
    {
        for (int i = 0; ; i++)
        {
            try
            {
                Result result = _handler.Handle(command);
                return result;
            }
            catch (Exception e)
            {
                if (i >= _databaseRetries && !IsDatabaseException(e))
                {
                    throw;
                }
            }
        }
    }

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
