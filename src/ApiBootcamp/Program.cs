using ApiBootcamp.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var seqServerUrl =
            context.Configuration["Seq:ServerUrl"]
            ?? "http://localhost:5341";

        var applicationName =
            context.Configuration["APPLICATION_NAME"]
            ?? "ApiBootcamp";

        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Async(writeTo =>
                writeTo.Console())
            .WriteTo.Async(writeTo =>
                writeTo.Seq(seqServerUrl));
    });

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "postgresql",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]);

    var app = builder.Build();

    if (app.Configuration.GetValue<bool>("Database:ApplyMigrations"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} respondió {StatusCode} en {Elapsed:0.0000} ms";

        options.GetLevel = (httpContext, elapsed, exception) =>
        {
            if (httpContext.Request.Path.StartsWithSegments("/health"))
            {
                return LogEventLevel.Debug;
            }

            if (exception is not null ||
                httpContext.Response.StatusCode >= 500)
            {
                return LogEventLevel.Error;
            }

            if (httpContext.Response.StatusCode >= 400)
            {
                return LogEventLevel.Warning;
            }

            return LogEventLevel.Information;
        };

        options.EnrichDiagnosticContext =
            (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set(
                    "RequestHost",
                    httpContext.Request.Host.Value);

                diagnosticContext.Set(
                    "RequestScheme",
                    httpContext.Request.Scheme);
            };
    });

    app.UseMiddleware<ValidationExceptionMiddleware>();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = _ => false
    });

    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = healthCheck =>
            healthCheck.Tags.Contains("ready")
    });

    Log.Information(
        "ApiBootcamp iniciada en entorno {Environment}",
        app.Environment.EnvironmentName);

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "ApiBootcamp finalizó inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}