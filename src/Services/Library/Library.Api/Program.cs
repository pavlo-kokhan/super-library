using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Library.Infrastructure;
using Library.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using SuperLibrary.Web.Extensions;
using SuperLibrary.Web.Filters;
using SuperLibrary.Web.Pipelines;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: "Library.Api", serviceVersion: "1.0.0"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddOtlpExporter();
    });

builder
    .Services
    .AddFluentValidationAutoValidation(configuration =>
    {
        configuration.DisableDataAnnotationsValidation = true;
    })
    .AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()], ServiceLifetime.Singleton)
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        configuration.Lifetime = ServiceLifetime.Scoped;
    })
    .AddValidationPipelines(Assembly.GetExecutingAssembly())
    .AddSingleton(typeof(IPipelineBehavior<,>), typeof(DefaultLoggingPipeline<,>))
    .AddDbContext<ApplicationDbContext>(options =>
    {
        var dataSource = new NpgsqlDataSourceBuilder(builder.Configuration["ConnectionStrings:Postgres"]!)
            .EnableDynamicJson()
            .Build();

        options.UseNpgsql(dataSource);
        options.ConfigureWarnings(warningsConfigurationBuilder 
            => warningsConfigurationBuilder
                .Ignore(RelationalEventId.PendingModelChangesWarning));
        
    })
    .AddControllers(options =>
    {
        options.Filters.Add<ModelValidationActionFilterAttribute>();
        options.Filters.Add<ResultableActionFilterAttribute>();
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .Services
    .AddScoped<DbSeeder>()
    .AddOpenApi();

var app = builder.Build();

if (app.Environment.IsEnvironment("Debug") || app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

await dbContext.Database.MigrateAsync();
await seeder.SeedAsync();

await app.RunAsync();