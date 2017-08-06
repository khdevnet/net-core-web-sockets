using Autofac;
using NightChat.Core.Http.Senders;

namespace NightChat.Core
{
    public class CoreAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpMessageSender>().As<IHttpMessageSender>();
        }
    }
}