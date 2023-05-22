using TimMovie.Core.Entities;
using TimMovie.Core.Services.Films;

namespace TimMovie.Web.GraphQL.Query;

[GraphQLName("FilmQuery")]
public class FilmQuery
{
    [GraphQLName("all")]
    [GraphQLNonNullType(false)]
    public async Task<List<Film>> GetAll([Service] FilmService filmService)
    {
        return await filmService.GetAllAsync();
    }
}