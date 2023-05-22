namespace TimMovie.Web.GraphQL.Query;

[GraphQLName("RootQuery")]
public class RootQuery
{
    [GraphQLName("films")]
    [GraphQLNonNullType(false)]
    public FilmQuery Film() => new();
}