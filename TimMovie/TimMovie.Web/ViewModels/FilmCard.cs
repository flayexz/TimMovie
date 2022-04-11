using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels;

public class FilmCard
{
    public Film Film { get; set; }
    public double Rating { get; set; }
    public bool IsExistInSubscribe { get; set; }
}