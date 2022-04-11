using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Services;

public abstract class FilmBuilder
{
    protected IQueryable<Film> Query;

    protected FilmBuilder(FilmBuilder builder)
    {
        ArgumentValidator.CheckOnNull(builder, nameof(builder));

        Query = builder.Query;
    }

    protected FilmBuilder(IQueryable<Film> query)
    {
        ArgumentValidator.CheckOnNull(query, nameof(query));

        Query = query;
    }

    public IEnumerable<Film> Execute(int amountTake, int amountSkip)
    {
        return Query
            .Skip(amountSkip)
            .Take(amountTake)
            .Include(film => film.Genres)
            .Include(film => film.Country)
            .ToList();
    }
}