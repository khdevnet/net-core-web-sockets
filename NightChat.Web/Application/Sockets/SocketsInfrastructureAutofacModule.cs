using Autofac;
using Microsoft.AspNetCore.Http;
using NightChat.Core.Sockets;

namespace NightChat.Infrastructure
{
    public class SocketsInfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SocketMessageProcessor>().As<ISocketMessageProcessor>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
    }
}