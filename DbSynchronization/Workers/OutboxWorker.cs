using DbSynchronization.Synchronizers.Students;
using Serilog;

namespace DbSynchronization.Workers;

public class OutboxWorker : BackgroundService
{
    private readonly StudentOutboxSynchronizer _studentOutboxSynchronizer;

    public OutboxWorker(StudentOutboxSynchronizer studentOutboxSynchronizer)
    {
        _studentOutboxSynchronizer = studentOutboxSynchronizer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Outbox synchronization worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _studentOutboxSynchronizer.Sync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "OutboxWorker caught the exception");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
