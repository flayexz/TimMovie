using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications;

/// <summary>
/// Фильтр на минимальный рейтинг.
/// </summary>
public class FilmWithMinimumRating: Specification<Film>
{
    /// <summary>
    /// Создает фильтр, который отбирает фильмы, рейтинг которых больше <paramref name="rating"/>.
    /// </summary>
    /// <param name="rating">Минимальный рейтинг.</param>
    public FilmWithMinimumRating(double rating)
    {
        Conditional = film =>
            film.UserFilmWatcheds.Select(watched => watched.Grade).Average() >= rating;
    }
}