namespace TimMovie.Web.ViewModels;

public record AnnualPeriod(int LastYear, int FirstYear)
{
    public bool IsOneYear => FirstYear == LastYear;
}