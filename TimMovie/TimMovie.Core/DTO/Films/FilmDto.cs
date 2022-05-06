using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO.Films;

public class FilmDto
{
    public string Title { get; set; }
    
    public int Year { get; set; }
    
    public string? Description { get; set; }
    
    public Country? Country { get; set; }
    public string? FilmLink { get; set; }
    public List<Comment> Comments { get; set; }

    public List<Producer> Producers { get; set; }

    public List<Actor> Actors { get; set; }

    public List<Genre> Genres { get; set; }
}