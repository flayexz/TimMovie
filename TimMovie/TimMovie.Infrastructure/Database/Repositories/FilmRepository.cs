using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Specifications;
using TimMovie.Core.Specifications.InheritedSpecifications;

namespace TimMovie.Infrastructure.Database.Repositories;

public class FilmRepository : Repository<Film>
{
    public FilmRepository(ApplicationContext context) : base(context)
    {
    }
    
    public async Task<Film?> FindByTitleAsync(string title) =>
        await _context.Films.FirstOrDefaultAsync(new FilmByNameCountrySpec(title));

}