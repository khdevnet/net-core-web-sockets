using Autofac;
using NightChat.DataAccess.Repositories;
using NightChat.Domain.Repositories;

namespace NightChat.DataAccess
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