using AutoMapper;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Person;

public class PersonService
{
    private readonly IRepository<Producer> _producerRepository;
    private readonly IRepository<Actor> _actorRepository;
    private readonly IMapper _mapper;

    public PersonService(IRepository<Producer> producerRepository, IRepository<Actor> actorRepository, IMapper mapper)
    {
        _producerRepository = producerRepository;
        _actorRepository = actorRepository;
        _mapper = mapper;
    }

    public PersonDto? GetPersonById(Guid id)
    {
        var person = _producerRepository.Query
            .FirstOrDefault(new EntityByIdSpec<Producer>(id));
        if (person is not null)
        {
            return _mapper.Map<PersonDto>(person);
        }
        
        var actor =  _actorRepository.Query.FirstOrDefault(new EntityByIdSpec<Actor>(id));
        return actor is not null
            ? _mapper.Map<PersonDto>(actor)
            : null;
    }

    public int GetAmountFilmsById(Guid id)
    {
        var countForActor = _actorRepository.Query
            .Where(new EntityByIdSpec<Actor>(id))
            .Select(actor => actor.Films.Count)
            .FirstOrDefault();
        var countForProducer = _producerRepository.Query
            .Where(new EntityByIdSpec<Producer>(id))
            .Select(actor => actor.Films.Count)
            .FirstOrDefault();

        return Math.Max(countForActor, countForProducer);
    }
}