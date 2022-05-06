using TimMovie.Core.DTO.Person;

namespace TimMovie.Core.DTO.Films;

public class FilmActorsAndProducersDto
{
    public IEnumerable<FilmProducerDto> FilmProducers { get; set; }
    public IEnumerable<FilmActorDto> FilmActors { get; set; }
}