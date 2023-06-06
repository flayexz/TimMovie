namespace TimMovie.Infrastructure.Settings;

public class KafkaSettings
{
    public const string SectionName = "Kafka";

    public string Host { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string Group { get; set; } = null!;
}