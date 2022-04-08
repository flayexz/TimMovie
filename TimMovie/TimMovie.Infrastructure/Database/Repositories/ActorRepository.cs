using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database.Repositories;

public class ActorRepository : Repository<Actor>
{
    private readonly ApplicationContext _context;

    public ActorRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Actor?> FindByNameAndSurnameAsync(string name, string surname) =>
        await _context.Actors.FirstOrDefaultAsync(a => a.Name.Equals(name) && a.Surname.Equals(surname));
}