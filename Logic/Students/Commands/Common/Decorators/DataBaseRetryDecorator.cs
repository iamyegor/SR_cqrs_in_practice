using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;

namespace Logic.Students.Commands.Common.Decorators;

public class DataBaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly IConfiguration _configuration;

    public DataBaseRetryDecorator(ICommandHandler<TCommand> handler, IConfiguration configuration)
    {
        _handler = handler;
        _configuration = configuration;
    }

    public Result Handle(TCommand command)
    {
        int databaseRetriesNumber = int.Parse(_configuration["DatabaseRetries"]!);

        for (int i = 0; ; i++)
        {
            try
            {
                Result result = _handler.Handle(command);
                return result;
            }
            catch (Exception e)
            {
                if (i >= databaseRetriesNumber && !IsDatabaseException(e))
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
