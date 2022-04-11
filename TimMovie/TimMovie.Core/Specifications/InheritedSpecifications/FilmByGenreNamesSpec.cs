using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Specifications.InheritedSpecifications;

/// <summary>
/// Фильтр по названию жанра фильма.
/// </summary>
public class FilmByGenreNamesSpec: Specification<Film>
{
    /// <summary>
    /// Фильтр, который отбирает все фильмы, которые принадлежат хотя бы одному жанру из списка.
    /// </summary>
    /// <param name="genreNames">Список имен жанров.</param>
    /// <exception cref="ArgumentNullException"><paramref name="genreNames"/> равен null.</exception>
    public FilmByGenreNamesSpec(string[] genreNames)
    {
        ArgumentValidator.CheckOnNull(genreNames, nameof(genreNames));
        
        Conditional = film => film.Genres.Select(genre => genre.Name).Intersect(genreNames).Any();
    }
}