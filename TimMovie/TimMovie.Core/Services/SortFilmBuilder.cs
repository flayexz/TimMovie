using TimMovie.Core.Entities;
using TimMovie.Core.MoreTypeFilms;

namespace TimMovie.Core.Services;

public class SortFilmBuilder : FilmBuilder
{
    public SortFilmBuilder(FilmBuilder builder) : base(builder)
    {
    }

    public SortFilmBuilder(IQueryable<Film> query) : base(query)
    {
    }
    
    public SortFilmBuilder SortByPopularity()
    {
        return this;
    }

    public SortFilmBuilder SortByRating()
    {
        Query = Query.OrderByDescending(film => film.UserFilmWatcheds.Select(watched => watched.Grade).Average());
        return this;
    }

    public SortFilmBuilder SortByReleaseDate()
    {
        Query = Query.OrderByDescending(film => film.Year);
        return this;
    }

    public SortFilmBuilder SortByViews()
    {
        Query = Query.OrderByDescending(film =>
            film.UserFilmWatcheds.Select(watched => watched.WatchedUser.Id).Distinct().Count());
        return this;
    }
    
    public SortFilmBuilder SortByFilmName()
    {
        Query = Query.OrderBy(film => film.Title);
        return this;
    }

    public SortFilmBuilder SortByType(TypeSortFilms typeSort)
    {
        switch (typeSort)
        {
           case TypeSortFilms.Popularity:
               return SortByPopularity();
           case TypeSortFilms.ReleaseDate:
               return SortByReleaseDate();
           case TypeSortFilms.Rating:
               return SortByRating();
           case TypeSortFilms.Views:
               return SortByViews();
           case TypeSortFilms.Title:
               return SortByFilmName();
           default:
               throw new ArgumentOutOfRangeException(nameof(typeSort), typeSort, null);
        }
    }
}