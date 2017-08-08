using Autofac;
using NightChat.Core.Sockets;

namespace NightChat.Sockets.Infrastructure
{
    public class SocketsInfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChatSocketMessageProcessor>().As<ISocketMessageProcessor>().SingleInstance();
        }
    }
}