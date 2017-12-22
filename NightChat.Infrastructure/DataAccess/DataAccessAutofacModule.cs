using Autofac;
using NightChat.Domain.Extensibility.Repositories;
using NightChat.Infrastructure.DataAccess.DataContext;
using NightChat.Infrastructure.DataAccess.Repositories;

namespace NightChat.Infrastructure.DataAccess
{
    public class DataAccessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokensRepository>().As<ITokensRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UsersRepository>().As<IUsersRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SessionDataContext>().As<ISessionDataContext>().SingleInstance();
        }
    }
}