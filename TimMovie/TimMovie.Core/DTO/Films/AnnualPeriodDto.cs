namespace TimMovie.Core.DTO.Films;

public record AnnualPeriodDto(int LastYear, int FirstYear)
{
    public bool IsOneYear => FirstYear == LastYear;
}