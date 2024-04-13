using DbSynchronization.Synchronizers.Students;
using Serilog;

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
        Log.Information("Student synchronization worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _studentSynchronizer.Sync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SynchronizerWorker caught the exception");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
