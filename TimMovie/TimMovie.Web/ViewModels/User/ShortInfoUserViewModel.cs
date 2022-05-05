using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities.Enums;

namespace TimMovie.Web.ViewModels.User;

public class ShortInfoUserViewModel
{
    public UserStatus Status { get; set; }
    public string DisplayName { get; set; }
    public string PathToPhoto { get; set; }
    public FilmForStatusDto? FilmForStatusDto { get; set; }
}