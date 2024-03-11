using Api.Utils;

var builder = WebApplication.CreateBuilder(args).ConfigureServices();

var app = builder.Build().ConfigureMiddlewares();

await app.ResetDatabaseAsync();

app.Run();
