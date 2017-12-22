using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using NightChat.Web.Extensibility.Sockets;
using NightChat.Web.Extensibility.Sockets.Models;

namespace NightChat.Web.Sockets
{
    internal class WebSocketConnectionManager : IWebSocketConnectionMannager
    {
        private readonly TimeSpan heartbeatTickRate = TimeSpan.FromSeconds(15);
        private readonly TimeSpan keepAlivePeriod = TimeSpan.FromSeconds(60);
        private readonly ConcurrentDictionary<string, SocketConnection> sockets = new ConcurrentDictionary<string, SocketConnection>();
        private Timer timer;

        public WebSocketConnectionManager()
        {
            timer = new Timer(Scan, this, heartbeatTickRate, heartbeatTickRate);
        }

        public SocketConnection GetSocketById(string id)
        {
            return sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, SocketConnection> GetAll()
        {
            return sockets;
        }

        public string GetId(SocketConnection socket)
        {
            return sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public bool TryAdd(string id, SocketConnection socket)
        {
            if (!String.IsNullOrWhiteSpace(id) && !sockets.TryGetValue(id, out var webSocket))
            {
                return sockets.TryAdd(id, socket);
            }
            return false;
        }

        public async Task Remove(string id, WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure)
        {
            if (sockets.TryRemove(id, out SocketConnection socket))
            {
                await socket.Socket.CloseOutputAsync(
                    closeStatus: closeStatus,
                    statusDescription: "Closed by the WebSocketManager",
                    cancellationToken: CancellationToken.None);
            }
        }

        private async void Scan()
        {
            foreach (KeyValuePair<string, SocketConnection> pairConnection in GetAll())
            {
                if ((DateTimeOffset.UtcNow - pairConnection.Value.LastSeen.ToUniversalTime()).TotalSeconds > keepAlivePeriod.TotalSeconds)
                {
                    await Remove(pairConnection.Key, WebSocketCloseStatus.EndpointUnavailable);
                }
            }
        }

        private static void Scan(object state)
        {
            ((WebSocketConnectionManager)state).Scan();
        }
    }
}