using System.Reflection;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students;
using DbSynchronization.Synchronizers.Students.Repositories;
using DbSynchronization.Utils;
using DbSynchronization.Workers;
using Mapster;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SynchronizerWorker>();
builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddTransient<StudentSynchronizer>();
builder.Services.AddTransient<StudentOutboxSynchronizer>();
builder.Services.AddTransient<CommandDbStudentRepository>();
builder.Services.AddTransient<CommandDbSynchronizationRepository>();
builder.Services.AddTransient<CommandDbOutboxRepository>();
builder.Services.AddTransient<QueryDbStudentRepository>();
builder.Services.AddTransient<SqlGenerator>();

CommandDbConnectionString.Value = builder.Configuration.GetConnectionString("Command")!;
QueryDbConnectionString.Value = builder.Configuration.GetConnectionString("Query")!;

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();

DapperConfiguration.ConfigureSnakeCaseMapping();

var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());

var host = builder.Build();
host.Run();
