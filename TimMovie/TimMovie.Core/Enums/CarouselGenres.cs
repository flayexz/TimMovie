using System.ComponentModel;

namespace TimMovie.Core.Enums;

public enum CarouselGenres
{
    [Description("Для детей")]
    ForKids,
    [Description("Семейные")]
    ForFamily=1,
    [Description("Комедии")]
    Comedy = 2,
    [Description("Драмы")]
    Dramas = 3,
    [Description("Триллеры")]
    Thrillers = 4
}