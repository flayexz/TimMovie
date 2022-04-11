using Autofac;
using TimMovie.Infrastructure.Database.Repositories;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure;

public class InfrastructureModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
    }
}