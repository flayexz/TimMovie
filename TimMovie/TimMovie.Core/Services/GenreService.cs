using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services;

public class GenreService
{
    private readonly IRepository<Genre> _genreRepository;

    public GenreService(IRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    public IEnumerable<string> GetGenreNames()
    {
        return _genreRepository.Query.Select(genre => genre.Name).ToList();
    }
}