using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimMovie.Infrastructure.Settings;

namespace TimMovie.Infrastructure.Configurations;

public static class MongoDbConfiguration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>();

            return new MongoClient(settings.Value.ConnectionString);
        });

        return services;
    }
}