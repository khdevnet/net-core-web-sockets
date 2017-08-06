using Autofac;
using NightChat.Domain.Services;

namespace NightChat.Domain
{
    public class DomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokensService>().As<ITokensService>();
            builder.RegisterType<UsersService>().As<IUsersService>();
        }
    }
}