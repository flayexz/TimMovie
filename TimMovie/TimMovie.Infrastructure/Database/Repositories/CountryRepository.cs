using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class CountryRepository : Repository<Country>
{
    public CountryRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<Country?> FindByNameAsync(string name) =>
        await Context.Countries.FirstOrDefaultAsync(g => g.Name.Equals(name));

}