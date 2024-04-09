using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students;
using DbSynchronization.Synchronizers.Students.Repositories;
using DbSynchronization.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SynchronizerWorker>();
builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddTransient<StudentSynchronizer>();
builder.Services.AddTransient<StudentOutboxSynchronizer>();
builder.Services.AddTransient<CommandDbStudentRepository>();
builder.Services.AddTransient<CommandDbSynchronizationRepository>();
builder.Services.AddTransient<CommandDbOutboxRepository>();
builder.Services.AddTransient<QueryDbStudentRepository>();

CommandDbConnectionString.Value = builder.Configuration.GetConnectionString("Command")!;
QueryDbConnectionString.Value = builder.Configuration.GetConnectionString("Query")!;

var host = builder.Build();
host.Run();
