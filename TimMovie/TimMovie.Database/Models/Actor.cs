using TimMovie.Database.BaseEntities;

namespace TimMovie.Database.Models;

public class Actor : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}