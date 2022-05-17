using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO;

public class SearchEntityResultDto
{
    public IEnumerable<Film> Films { get; set; }
    
    public IEnumerable<Genre> Genres { get; set; }
    
    public IEnumerable<Actor> Actors { get; set;  }
    
    public IEnumerable<Producer> Producers { get; set; }
}