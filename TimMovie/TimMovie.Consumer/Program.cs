using TimMovie.Consumer;
using TimMovie.Infrastructure.Configurations;
using TimMovie.Infrastructure.Settings;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<KafkaSettings>(ctx.Configuration.GetRequiredSection(KafkaSettings.SectionName));
        services.Configure<MongoSettings>(ctx.Configuration.GetRequiredSection(MongoSettings.SectionName));
        services.AddKafkaAdminBuilder();
        services.AddKafkaConsumer();
        services.AddMongoDb();
        services.AddHostedService<FilmTrafficConsumer>();
    })
    .Build();

host.Run();