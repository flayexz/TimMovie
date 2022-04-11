using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services;

public class FilmsFilterService
{
    private readonly IRepository<Film> _filmRepository;

    public FilmsFilterService(IRepository<Film> filmRepository)
    {
        _filmRepository = filmRepository;
    }

    public FilteredFilmsBuilder CreateFilterBuilder(FilmBuilder? filterBuilder = null)
    {
        return filterBuilder is null
            ? new FilteredFilmsBuilder(_filmRepository.Query)
            : new FilteredFilmsBuilder(filterBuilder);
    }

    public SortFilmBuilder CreateSortBuilder(FilmBuilder? filterBuilder = null)
    {
        return filterBuilder is null
            ? new SortFilmBuilder(_filmRepository.Query)
            : new SortFilmBuilder(filterBuilder);
    }
}