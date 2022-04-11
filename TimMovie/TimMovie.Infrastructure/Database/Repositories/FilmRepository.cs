using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class FilmRepository : Repository<Film>
{
    private readonly ApplicationContext _context;

    public FilmRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Film?> FindByTitleAsync(string title) =>
        await _context.Films.FirstOrDefaultAsync(g => g.Title.Equals(title));
}