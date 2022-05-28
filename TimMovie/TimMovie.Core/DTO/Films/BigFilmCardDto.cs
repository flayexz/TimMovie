using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO.Films;

public class BigFilmCardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string? Description { get; set; }
    public string CountryName { get; set; } = null!;
    public string Image { get; set; } = null!;
    public List<Genre> Genres { get; set; }
    public List<Actor> Actors { get; set; }
    public Producer? Producer { get; set; }
    public double? Rating { get; set; }
}