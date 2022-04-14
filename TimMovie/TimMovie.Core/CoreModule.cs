using Autofac;
using TimMovie.Core.Services;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;

namespace TimMovie.Core;

public class CoreModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterServiceOnSelf<FilmService>(builder);
        RegisterServiceOnSelf<CountryService>(builder);
        RegisterServiceOnSelf<GenreService>(builder);
        RegisterServiceOnSelf<FilmCardService>(builder);
    }

    private void RegisterServiceOnSelf<T>(ContainerBuilder builder) 
        where T : notnull
    {
        builder.RegisterType<T>().AsSelf().InstancePerLifetimeScope();
    }
}