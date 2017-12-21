using System;

namespace NightChat.Web.Application.Extensibility.Sockets
{
    public interface IKeepAliveTimer
    {
        void Start(int dueTime, int period);

        void Execute(Object stateInfo);
    }
}