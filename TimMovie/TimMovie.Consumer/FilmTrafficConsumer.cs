using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimMovie.Core.Entities;
using TimMovie.Core.Events;
using TimMovie.Infrastructure.Settings;

namespace TimMovie.Consumer;

public class FilmTrafficConsumer : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IConsumer<Ignore, RegisterFilmTrafficEvent> _consumer;
    private readonly ILogger<FilmTrafficConsumer> _logger;
    private readonly IAdminClient _adminClient;
    private readonly IMongoClient _mongoClient;
    private readonly MongoSettings _mongoSettings;

    public FilmTrafficConsumer(
        IOptions<KafkaSettings> kafkaSettings,
        IConsumer<Ignore, RegisterFilmTrafficEvent> consumer,
        ILogger<FilmTrafficConsumer> logger,
        IAdminClient adminClient,
        IMongoClient mongoClient,
        IOptions<MongoSettings> mongoSettings)
    {
        _consumer = consumer;
        _logger = logger;
        _adminClient = adminClient;
        _mongoClient = mongoClient;
        _mongoSettings = mongoSettings.Value;
        _kafkaSettings = kafkaSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await CreateTopicAsync(stoppingToken);

            _consumer.Subscribe(_kafkaSettings.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;

                var mongoDatabase = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
                var collection = mongoDatabase.GetCollection<FilmTraffic>(_mongoSettings.FilmTrafficCollectionName);

                var existingFilmTraffic = await
                    (await collection.FindAsync(f => f.FilmId == message.Id, cancellationToken: stoppingToken))
                    .FirstOrDefaultAsync(cancellationToken: stoppingToken);

                if (existingFilmTraffic is null)
                {
                    var traffic = new FilmTraffic
                    {
                        FilmId = message.Id,
                        Count = 1
                    };

                    await collection.InsertOneAsync(traffic, cancellationToken: stoppingToken);
                }
                else
                {
                    var update = Builders<FilmTraffic>.Update.Inc(f => f.Count, 1);

                    await collection.UpdateOneAsync(f => f.FilmId == message.Id, update,
                        cancellationToken: stoppingToken);
                }
            }
        }
        catch (Exception)
        {
            _consumer.Close();
            throw;
        }
    }

    private async Task CreateTopicAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            try
            {
                await _adminClient.CreateTopicsAsync(new List<TopicSpecification>
                {
                    new()
                    {
                        Name = _kafkaSettings.Topic
                    }
                });
                break;
            }
            catch (CreateTopicsException ex)
            {
                if (ex.Results.Any(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
                {
                    break;
                }

                _logger.LogWarning("Try create topic. Retry");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}