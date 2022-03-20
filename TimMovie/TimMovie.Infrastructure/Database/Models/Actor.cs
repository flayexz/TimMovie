namespace TimMovie.Infrastructure.Database;

public class Actor : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}