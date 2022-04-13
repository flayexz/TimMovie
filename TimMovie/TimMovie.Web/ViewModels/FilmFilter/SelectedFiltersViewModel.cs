using TimMovie.Core.Enums;

namespace TimMovie.Web.ViewModels.FilmFilter;

public class SelectedFiltersViewModel
{
    public FilmSortingType SortingType { get; set; }
    public IEnumerable<string> GenresName { get; set; }
    public IEnumerable<string> CountriesName { get; set; }
    public int? Rating { get; set; }
    public AnnualPeriodViewModel AnnualPeriodsViewModel { get; set; }
}