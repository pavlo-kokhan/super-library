using Notification.Api.Extensions;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: "Notification.Api", serviceVersion: "1.0.0"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddRabbitMQInstrumentation()
            .AddOtlpExporter();
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddConsumers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();