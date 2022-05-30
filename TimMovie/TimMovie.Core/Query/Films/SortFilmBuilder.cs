using System.Linq.Expressions;
using TimMovie.Core.Entities;
using TimMovie.Core.Enums;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Query.Films;

public class SortFilmBuilder : FilmQueryBuilder
{
    public SortFilmBuilder(FilmQueryBuilder builder) : base(builder)
    {
    }

    public SortFilmBuilder(IRepository<Film> repository) : base(repository)
    {
    }

    public SortFilmBuilder AddSortByPopularity(bool isDescending)
    {
        AddSort(isDescending, film => film);
        
        return this;
    }

    public SortFilmBuilder AddSortByRating(bool isDescending)
    {
        AddSort(
            isDescending, 
            film => film.UserFilmWatcheds.Select(watched => watched.Grade).Average());
        
        return this;
    }

    public SortFilmBuilder AddSortByReleaseDate(bool isDescending)
    {
        AddSort(isDescending, film => film.Year);
        
        return this;
    }

    public SortFilmBuilder AddSortByViews(bool isDescending)
    {
        AddSort(isDescending, film =>
            film.UserFilmWatcheds.Select(watched => watched.WatchedUser.Id).Distinct().Count());
        
        return this;
    }

    public SortFilmBuilder AddSortByFilmName(bool isDescending)
    {
        AddSort(isDescending, film => film.Title);
        
        return this;
    }

    public SortFilmBuilder AddSortByTypeSort(FilmSortingType sortType, bool isDescending)
    {
        switch (sortType)
        {
            case FilmSortingType.Popularity:
                return AddSortByPopularity(isDescending);
            case FilmSortingType.ReleaseDate:
                return AddSortByReleaseDate(isDescending);
            case FilmSortingType.Rating:
                return AddSortByRating(isDescending);
            case FilmSortingType.Views:
                return AddSortByViews(isDescending);
            case FilmSortingType.Title:
                return AddSortByFilmName(isDescending);
            default:
                throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null);
        }
    }
    
    private void AddSort<TKey>(bool isDescending, Expression<Func<Film, TKey>> expression)
    {
        Query = isDescending
            ? Query.OrderByDescending(expression)
            : Query.OrderBy(expression);
    }
}