namespace TimMovie.Web.ViewModels.FilmFilter;

public record AnnualPeriodViewModel(int LastYear, int FirstYear)
{
    public bool IsOneYear => FirstYear == LastYear;
}