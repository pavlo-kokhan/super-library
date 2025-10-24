using Booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using SuperLibrary.Web.Helpers;

namespace Booking.Api.Extensions;

public static class ServiceCollectionDbExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
        => services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                var dataSource = new NpgsqlDataSourceBuilder(
                        DotNetEnvHelper.GetEnvironmentVariableOrThrow("DB_CONNECTION_STRING"))
                        .EnableDynamicJson()
                        .Build();

                options.UseNpgsql(dataSource);
                options.ConfigureWarnings(warningsConfigurationBuilder =>
                    warningsConfigurationBuilder.Ignore(RelationalEventId.PendingModelChangesWarning));

            });
}