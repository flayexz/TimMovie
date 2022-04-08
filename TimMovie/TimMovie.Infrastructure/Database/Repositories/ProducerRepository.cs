using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class ProducerRepository : Repository<Producer>
{
    private readonly ApplicationContext _context;

    public ProducerRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Producer?> FindByNameAndSurnameAsync(string name, string surname) =>
        await _context.Producers.FirstOrDefaultAsync(p => p.Name.Equals(name) && p.Surname.Equals(surname));
}