using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TimMovie.Core.Events;
using TimMovie.Infrastructure.Serializers;
using TimMovie.Infrastructure.Settings;

namespace TimMovie.Infrastructure.Configurations;

public static class KafkaConfiguration
{
    public static IServiceCollection AddKafkaAdminBuilder(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var kafkaSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
            var config = new AdminClientConfig
            {
                BootstrapServers = kafkaSettings.Host
            };
            return new AdminClientBuilder(config).Build();
        });
    }

    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var kafkaSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Host,
                GroupId = kafkaSettings.Group,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Ignore, RegisterFilmTrafficEvent>(config)
                .SetValueDeserializer(new RegisterFilmTrafficEventSerializer())
                .Build();
        });
    }

    public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var kafkaSettings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Host
            };

            return new ProducerBuilder<Null, RegisterFilmTrafficEvent>(config)
                .SetValueSerializer(new RegisterFilmTrafficEventSerializer())
                .Build();
        });
    }
}