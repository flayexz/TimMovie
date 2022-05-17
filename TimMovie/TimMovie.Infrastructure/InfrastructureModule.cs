using Autofac;
using Microsoft.Extensions.Configuration;
using TimMovie.Infrastructure.Services;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Core.Interfaces;
using TimMovie.Infrastructure.Database;

namespace TimMovie.Infrastructure;

public class InfrastructureModule: Module
{
    private readonly IConfiguration _configuration;
    
    public InfrastructureModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        builder.RegisterType<MailKitService>().As<IMailService>().InstancePerLifetimeScope();
        builder.RegisterType<IpService>().As<IIpService>().InstancePerDependency();
        builder.RegisterType<UserMessageService>().As<IUserMessageService>().InstancePerLifetimeScope();
        builder.RegisterType<VkService>().As<IVkService>().WithParameters(new[]
        {
            new NamedParameter("accessToken", _configuration["VkSettings:AccessToken"]),
            new NamedParameter("client", new HttpClient())
        }).InstancePerDependency();
        builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();
        builder.RegisterType<ConfigurationService>().As<IConfigurationService>().InstancePerLifetimeScope();
        builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
    }
}