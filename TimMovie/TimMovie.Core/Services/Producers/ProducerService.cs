using AutoMapper;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.ProducerSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Producers;

public class ProducerService
{
    private readonly IRepository<Producer> _producerRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly IMapper _mapper;

    public ProducerService(
        IRepository<Producer> producerRepository, 
        IRepository<Film> filmRepository,
        IMapper mapper)
    {
        _producerRepository = producerRepository;
        _filmRepository = filmRepository;
        _mapper = mapper;
    }

    public IEnumerable<Producer> GetProducersByNamePart(string namePart, int count = int.MaxValue) =>
        _producerRepository.Query
            .Where(new ProducerByNamePartSpec(namePart))
            .Take(count);

    public IEnumerable<FilmProducerDto> GetFilmProducers(Guid filmId)
    {
        var query = _filmRepository.Query.Where(new EntityByIdSpec<Film>(filmId));
        var film = 
            new QueryExecutor<Film>(query, _filmRepository)
                .IncludeInResult(f => f.Producers)
                .FirstOrDefault();
        if (film is null)
        {
            return Enumerable.Empty<FilmProducerDto>();
        }
        
        var producers = _mapper.Map<IEnumerable<FilmProducerDto>>(film.Producers);
        return producers;
    }
}