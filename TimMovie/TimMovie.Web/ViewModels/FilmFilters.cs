using TimMovie.Core.MoreTypeFilms;

namespace TimMovie.Web.ViewModels;

public class FilmFilters
{
    public IEnumerable<string> GenresName { get; set; }
    public IEnumerable<string> CountriesName { get; set; }
    public IEnumerable<int> Ratings { get; set; }
    public IEnumerable<AnnualPeriod> AnnualPeriods { get; set; }
}

public class ResultFilters
{
    public TypeSortFilms TypeFilter { get; set; }
    public IEnumerable<string> GenresName { get; set; }
    public IEnumerable<string> CountriesName { get; set; }
    public int? Rating { get; set; }
    public AnnualPeriod AnnualPeriods { get; set; }
}