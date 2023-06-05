using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.Settings;

namespace TimMovie.Web.Background;

public class StatisticsBackgroundService : BackgroundService
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoSettings _mongoSettings;

    public StatisticsBackgroundService(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        _mongoClient = mongoClient;
        _mongoSettings = mongoSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672")
        };

        using var channel = factory.CreateConnection().CreateModel();

        while (!stoppingToken.IsCancellationRequested)
        {
            var mongoDatabase = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
            var collection = mongoDatabase.GetCollection<FilmTraffic>(_mongoSettings.FilmTrafficCollectionName);

            var traffics = collection
                .AsQueryable()
                .Select(x => new
                {
                    Id = x.FilmId, x.Count
                })
                .ToList();

            var dict = traffics.ToDictionary(e => e.Id, e => e.Count);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dict));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.CorrelationId = Guid.NewGuid().ToString();

            channel.ExchangeDeclare("statistics-exchange", "fanout");
            channel.BasicPublish(exchange: "statistics-exchange", routingKey: "", basicProperties: properties,
                body: body);
            await Task.Delay(1000, stoppingToken);
        }
    }
}