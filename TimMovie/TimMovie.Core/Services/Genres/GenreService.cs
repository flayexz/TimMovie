using System.ComponentModel;
using System.Reflection;
using TimMovie.Core.Entities;
using TimMovie.Core.Enums;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Genres;

public class GenreService
{
    private readonly IRepository<Genre> _genreRepository;

    public GenreService(IRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public IEnumerable<string> GetGenreNames()
    {
        return _genreRepository.Query.Select(genre => genre.Name).ToList();
    }

    public string GetGenreNameByEnumName(CarouselGenres genre)
    {
        Type type = genre.GetType();

        MemberInfo[] memInfo = type.GetMember(genre.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute) attrs[0]).Description;
        }

        return genre.ToString();
    }
}