using Api.Utils;
using DTOs;
using Logic.DAL;
using Logic.Students;
using Logic.Students.Commands.Common;
using Logic.Students.Commands.Common.Decorators;
using Logic.Students.Commands.Disenroll;
using Logic.Students.Commands.EditPersonalInfo;
using Logic.Students.Commands.Enroll;
using Logic.Students.Commands.Register;
using Logic.Students.Commands.Transfer;
using Logic.Students.Commands.Unregister;
using Logic.Students.Queries.Common;
using Logic.Students.Queries.GetStudentsList;
using Logic.Utils;
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

        builder.Services.AddTransient<Messages>();
        // builder.Services.AddCqrsHandlersFromAssemblyContaining<ILogicAssembly>();

        builder.Services.AddSingleton<ExceptionIncrementor>();

        builder.Services.AddTransient<ICommandHandler<EditPersonalInfoCommand>>(
            provider => new DataBaseRetryDecorator<EditPersonalInfoCommand>(
                new EditPersonalInfoCommandHandler(
                    provider.GetRequiredService<StudentRepository>(),
                    provider.GetRequiredService<ExceptionIncrementor>()
                ),
                provider.GetRequiredService<IConfiguration>()
            )
        );

        builder.Services.AddTransient<Messages>();
        builder.Services.AddTransient<
            IQueryHandler<GetStudentsListQuery, IEnumerable<StudentDto>>,
            GetStudentsListQueryHandler
        >();

        builder.Services.AddTransient<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
        builder.Services.AddTransient<
            ICommandHandler<UnregisterCommand>,
            UnregisterCommandHandler
        >();
        builder.Services.AddTransient<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
        builder.Services.AddTransient<ICommandHandler<TransferCommand>, TransferCommandHandler>();
        builder.Services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();

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
