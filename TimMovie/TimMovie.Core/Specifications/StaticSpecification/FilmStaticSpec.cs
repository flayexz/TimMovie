using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.StaticSpecification;

public class FilmStaticSpec
{
    public static readonly Specification<Film> FilmIsIncludedAnySubscriptionSpec = 
        new(film => film.Subscribes.Any());
}