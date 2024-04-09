using DbSynchronization.Synchronizers.Students;

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
        Console.WriteLine("Outbox synchronization worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            _studentOutboxSynchronizer.Sync();

            await Task.Delay(1000, stoppingToken);
        }
    }
}
