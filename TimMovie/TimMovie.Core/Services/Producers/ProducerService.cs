using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Producers;

public class ProducerService
{
    private readonly IRepository<Producer> _producerRepository;

    public ProducerService(IRepository<Producer> producerRepository)
    {
        _producerRepository = producerRepository;
    }

    public IEnumerable<Producer> GetProducersByNamePart(string namePart, int count = int.MaxValue) =>
        _producerRepository.Query
            .Where(p => p.Surname == null ? p.Name.Contains(namePart) : (p.Name + " " + p.Surname).Contains(namePart))
            .Take(count);
}