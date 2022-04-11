using TimMovie.Web.Models;

namespace TimMovie.Web.ViewModels;

public class BannerViewModel
{
    public string? Description { get; set; }

    public string Image { get; set; }

    public FilmMainPageViewModel Film { get; set; }

    public BannerViewModel(string? description, string image, FilmMainPageViewModel film)
    {
        Description = description;
        Image = image;
        Film = film;
    }
}