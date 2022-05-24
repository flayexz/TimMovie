using AutoMapper;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec.ActorSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Actors;

public class ActorService
{
    private readonly IRepository<Actor> _actorRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly IMapper _mapper;

    public ActorService(IRepository<Actor> actorRepository, IRepository<Film> filmRepository, IMapper mapper)
    {
        _actorRepository = actorRepository;
        _filmRepository = filmRepository;
        _mapper = mapper;
    }

    public IEnumerable<Actor> GetActorsByNamePart(string? namePart, int count = int.MaxValue) =>
        _actorRepository.Query
            .Where(new ActorByNamePartSpec(namePart))
            .Take(count);
    
    public IEnumerable<FilmActorDto> GetFilmActors(Guid filmId)
    {
        var query = _filmRepository.Query.Where(new EntityByIdSpec<Film>(filmId));
        var film = 
            new QueryExecutor<Film>(query, _filmRepository)
                .IncludeInResult(f => f.Actors)
                .FirstOrDefault();
        if (film is null)
        {
            return Enumerable.Empty<FilmActorDto>();
        }
        
        var producers = _mapper.Map<IEnumerable<FilmActorDto>>(film.Actors);
        return producers;
    }
}