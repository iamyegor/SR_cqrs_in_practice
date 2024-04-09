using DbSynchronization.Synchronizers.Students;

namespace DbSynchronization.Workers;

public class SynchronizerWorker : BackgroundService
{
    private readonly StudentSynchronizer _studentSynchronizer;

    public SynchronizerWorker(StudentSynchronizer studentSynchronizer)
    {
        _studentSynchronizer = studentSynchronizer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Student synchronization worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            _studentSynchronizer.Sync();

            await Task.Delay(1000, stoppingToken);
        }
    }
}
