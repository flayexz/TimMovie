using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Services;

public class FilteredFilmsBuilder: FilmBuilder
{
    
    public FilteredFilmsBuilder(FilmBuilder builder) : base(builder)
    {
    }
    
    public FilteredFilmsBuilder(IQueryable<Film> query) : base(query)
    {
    }

    public FilteredFilmsBuilder FilterByGenre(IEnumerable<string> genreNames)
    {
        if (genreNames is null)
        {
            return this;
        }
        
        Query = Query.Where(new FilmByGenreNamesSpec(genreNames.ToArray()));
        return this;
    }
    
    public FilteredFilmsBuilder FilterByYear(int year)
    {
        Query = Query.Where(new FilmDateSpec(year));
        return this;
    }
    
    public FilteredFilmsBuilder FilterByYear(int firstYear, int lastYear)
    {
        Query = Query.Where(new FilmDateSpec(firstYear, lastYear));
        return this;
    }
    
    public FilteredFilmsBuilder FilterByCountry(IEnumerable<string> countryNames)
    {
        if (countryNames is null || !countryNames.Any())
        {
            return this;
        }

        Specification<Film> filterOnCountry = new FilmByNameCountrySpec(countryNames.First());
        foreach (var name in countryNames.Skip(1))
        {
            filterOnCountry = filterOnCountry || new FilmByNameCountrySpec(name);
        }

        Query = Query.Where(filterOnCountry);
        
        return this;
    }
    
    public FilteredFilmsBuilder FilterOnMinimumRating(int rating)
    {
        Query = Query.Where(new FilmWithMinimumRating(rating));
        return this;
    }
}