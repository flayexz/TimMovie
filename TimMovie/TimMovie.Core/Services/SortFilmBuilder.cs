using TimMovie.Core.Entities;
using TimMovie.Core.Enums;

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

    public SortFilmBuilder SortByType(FilmSortingType sortType)
    {
        switch (sortType)
        {
           case FilmSortingType.Popularity:
               return SortByPopularity();
           case FilmSortingType.ReleaseDate:
               return SortByReleaseDate();
           case FilmSortingType.Rating:
               return SortByRating();
           case FilmSortingType.Views:
               return SortByViews();
           case FilmSortingType.Title:
               return SortByFilmName();
           default:
               throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null);
        }
    }
}