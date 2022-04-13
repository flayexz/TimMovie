using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels.Films;

public class FilmCardViewModel
{
    public Film Film { get; set; }
    public double Rating { get; set; }
    public bool IsExistInSubscribe { get; set; }
}