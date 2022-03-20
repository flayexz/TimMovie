namespace TimMovie.Infrastructure.Database;

public class Producer : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}