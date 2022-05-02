using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities.Enums;

namespace TimMovie.Core.DTO.Users;

public class ShortInfoUserDto
{
    public UserStatus Status { get; set; }
    public string DisplayName { get; set; }
    public string PathToPhoto { get; set; }
    public FilmForStatusDto? FilmForStatusDto { get; set; }
}