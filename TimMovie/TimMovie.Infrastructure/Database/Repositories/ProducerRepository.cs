using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class ProducerRepository : Repository<Producer>
{
    public ProducerRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<Producer?> FindByNameAndSurnameAsync(string name, string surname) =>
        await Context.Producers.FirstOrDefaultAsync(p => p.Name.Equals(name) && p.Surname.Equals(surname));
}