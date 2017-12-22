using System;
using System.Net.WebSockets;

namespace NightChat.Web.Application.Extensibility.Sockets.Models
{
    public class SocketConnection
    {
        public SocketConnection(WebSocket socket)
        {
            Socket = socket;
        }

        public WebSocket Socket { get; }

        public DateTime LastSeen { get; set; } = DateTime.Now;
    }
}