using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class FilmRepository : Repository<Film>
{
    public FilmRepository(ApplicationContext context) : base(context)
    {
    }
    
    public async Task<Film?> FindByTitleAsync(string title) =>
        await _context.Films.FirstOrDefaultAsync(g => g.Title.Equals(title));

}