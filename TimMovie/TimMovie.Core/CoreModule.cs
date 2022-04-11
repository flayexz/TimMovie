using Autofac;
using TimMovie.Core.Services;

namespace TimMovie.Core;

public class CoreModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FilmService>().AsSelf();
        builder.RegisterType<FilmsFilterService>().AsSelf();
    }
}