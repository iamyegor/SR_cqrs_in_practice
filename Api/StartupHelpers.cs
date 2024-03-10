using Api.DTOs;
using Api.Utils;
using Logic.DAL;
using Logic.Students;
using Logic.Students.Commands.Common;
using Logic.Students.Commands.EditPersonalInfo;
using Logic.Students.Queries.Common;
using Logic.Students.Queries.GetStudentsList;
using Microsoft.EntityFrameworkCore;

namespace Api;

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
            options.UseNpgsql(
                "Host=localhost;Port=5432;Username=postgres;Password=yapidr;Database=khorikov_cqrs;Include Error Detail=true"
            );
        });

        builder.Services.AddTransient<StudentRepository>();
        builder.Services.AddTransient<CourseRepository>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddTransient<
            ICommandHandler<EditPersonalInfoCommand>,
            EditPersonalInfoCommandHandler
        >();

        builder.Services.AddTransient<Messages>();
        builder.Services.AddTransient<
            IQueryHandler<GetStudentsListQuery, IEnumerable<StudentDto>>,
            GetStudentsListQueryHandler
        >();

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
