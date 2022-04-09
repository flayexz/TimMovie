using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications;

public class FilmCountrySpec: Specification<Film>
{
    public FilmCountrySpec(string countryName)
    {
        Conditional = film => film.Country!.Name == countryName;
    }
}