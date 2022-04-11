using Autofac;
using TimMovie.Core.Services;

namespace TimMovie.Core;

public class CoreModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterServiceOnSelf<FilmService>(builder);
        RegisterServiceOnSelf<FilmsFilterService>(builder);
        RegisterServiceOnSelf<CountryService>(builder);
        RegisterServiceOnSelf<GenreService>(builder);
    }

    private void RegisterServiceOnSelf<T>(ContainerBuilder builder) 
        where T : notnull
    {
        builder.RegisterType<T>().AsSelf().InstancePerLifetimeScope();
    }
}