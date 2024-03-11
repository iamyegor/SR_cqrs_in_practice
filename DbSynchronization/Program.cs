using DbSynchronization;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers;
using DbSynchronization.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SynchronizerWorker>();

builder.Services.AddTransient<StudentSynchronizer>();

CommandDbConnectionString.Value = builder.Configuration.GetConnectionString("Command")!;

var host = builder.Build();
host.Run();
