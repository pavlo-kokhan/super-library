using DotNetEnv;
using Notification.Api.Extensions;

Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "config.env"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddRabbitMq();
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