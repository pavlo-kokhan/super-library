using System.Reflection;
using System.Text.Json.Serialization;
using Booking.Api.Application.Services;
using Booking.Api.Application.Services.Abstract;
using Booking.Api.Constants;
using Booking.Api.Extensions;
using Booking.Infrastructure;
using Booking.Infrastructure.Persistence;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SuperLibrary.Web.Extensions;
using SuperLibrary.Web.Filters;
using SuperLibrary.Web.Helpers;
using SuperLibrary.Web.Pipelines;

Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "config.env"));

var builder = WebApplication.CreateBuilder(args);

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
    .AddApplicationDbContext()
    .AddControllers(options =>
    {
        options.Filters.Add<ModelValidationActionFilterAttribute>();
        options.Filters.Add<ResultableActionFilterAttribute>();
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .Services
    .AddScoped<DbSeeder>()
    .AddOpenApi()
    .AddRabbitMq()
    .AddPublishers()
    .AddScoped<ILibraryService, LibraryService>()
    .AddHttpClient(HttpClientNames.Library,
        client =>
        {
            client.BaseAddress = new Uri(DotNetEnvHelper.GetEnvironmentVariableOrThrow("LIBRARY_BASE_URL"));
        });

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