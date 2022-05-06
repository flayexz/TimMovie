﻿using AutoMapper;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Actors;

public class ActorService
{
    private readonly IRepository<Actor> _actorRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly Mapper _mapper;

    public ActorService(IRepository<Actor> actorRepository, IRepository<Film> filmRepository, Mapper mapper)
    {
        _actorRepository = actorRepository;
        _filmRepository = filmRepository;
        _mapper = mapper;
    }

    public IEnumerable<Actor> GetActorsByNamePart(string namePart, int count = int.MaxValue) =>
        _actorRepository.Query
            .Where(a => a.Surname == null ? a.Name.Contains(namePart) : (a.Name + " " + a.Surname).Contains(namePart))
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