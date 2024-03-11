using Logic.DAL;
using Logic.Students;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Api.Utils;

public static class StartupHelpers
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder
            .Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("myString"));
        });

        builder.Services.AddTransient<StudentRepository>();
        builder.Services.AddTransient<CourseRepository>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddTransient<Messages>();
        builder.Services.AddCqrsHandlers();
        builder.Services.AddSingleton<ExceptionIncrementor>();

        return builder;
    }

    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseMiddleware<ExceptionHandler>();

        return app;
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                DbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured when resetting the database: {e.Message}");
            }
        }
    }
}
