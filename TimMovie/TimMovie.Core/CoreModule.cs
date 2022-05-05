using Autofac;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Actors;
using TimMovie.Core.Services.Banners;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Core.Services.Producers;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Core.Services.SupportedServices;
using TimMovie.Core.ValidatorServices;

namespace TimMovie.Core;

public class CoreModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterServiceOnSelf<FilmService>(builder);
        RegisterServiceOnSelf<CountryService>(builder);
        RegisterServiceOnSelf<GenreService>(builder);
        RegisterServiceOnSelf<FilmCardService>(builder);
        RegisterServiceOnSelf<BannerService>(builder);
        RegisterServiceOnSelf<ActorService>(builder);
        RegisterServiceOnSelf<ProducerService>(builder);
        RegisterServiceOnSelf<SubscribeService>(builder);
        RegisterServiceOnSelf<FileService>(builder);
        RegisterServiceOnSelf<UserValidator>(builder);
    }

    private void RegisterServiceOnSelf<T>(ContainerBuilder builder) 
        where T : notnull
    {
        builder.RegisterType<T>().AsSelf().InstancePerLifetimeScope();
    }
}