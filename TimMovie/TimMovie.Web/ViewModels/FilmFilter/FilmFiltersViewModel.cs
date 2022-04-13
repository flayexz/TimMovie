namespace TimMovie.Web.ViewModels.FilmFilter;

public class FilmFiltersViewModel
{
    public IEnumerable<string> GenresName { get; set; }
    public IEnumerable<string> CountriesName { get; set; }
    public IEnumerable<int> Ratings { get; set; }
    public IEnumerable<AnnualPeriodViewModel> AnnualPeriods { get; set; }
}