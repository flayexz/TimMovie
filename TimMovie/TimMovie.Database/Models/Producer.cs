using TimMovie.Database.BaseEntities;

namespace TimMovie.Database.Models;

public class Producer : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}