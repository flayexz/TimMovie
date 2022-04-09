using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class GenreRepository : Repository<Genre>
{
    public GenreRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<Genre?> FindByNameAsync(string name) =>
        await _context.Genres.FirstOrDefaultAsync(g => g.Name.Equals(name));
}