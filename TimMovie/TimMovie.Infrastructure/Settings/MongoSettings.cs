namespace TimMovie.Infrastructure.Settings;

public sealed class MongoSettings
{
    public const string SectionName = "Mongo";

    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string FilmTrafficCollectionName { get; set; } = null!;
}