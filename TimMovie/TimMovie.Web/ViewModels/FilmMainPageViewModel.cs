using TimMovie.Web.Models;

namespace TimMovie.Web.ViewModels;

public class FilmMainPageViewModel
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string? Image { get; set; }
    
    public double? Rating { get; set; }
}