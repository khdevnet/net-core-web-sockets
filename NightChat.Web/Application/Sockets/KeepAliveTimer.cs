using System;
using System.Threading;
using NightChat.Web.Application.Extensibility.Sockets;

namespace NightChat.Web.Application.Sockets
{
    public class KeepAliveTimer : IKeepAliveTimer
    {
        private Timer internalTimer;
        private AutoResetEvent autoEvent;

        public void Start(int dueTime, int period)
        {
            internalTimer = new Timer(Execute, autoEvent, dueTime, period);
            autoEvent = new AutoResetEvent(false);
        }

        public void Execute(Object stateInfo)
        {
            internalTimer.Dispose();
        }
    }
}