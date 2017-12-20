using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace NightChat.Web.Application.Sockets
{
    internal class WebSocketConnectionManager : IWebSocketConnectionMannager
    {
        private readonly ConcurrentDictionary<string, WebSocket> sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return sockets;
        }

        public string GetId(WebSocket socket)
        {
            return sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public bool TryAdd(string id, WebSocket socket)
        {
            if (!String.IsNullOrWhiteSpace(id) && !sockets.TryGetValue(id, out var webSocket))
            {
                return sockets.TryAdd(id, socket);
            }
            return false;
        }

        public async Task Remove(string id)
        {
            WebSocket socket;
            if (sockets.TryRemove(id, out socket))
            {
                await socket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Closed by the WebSocketManager",
                    cancellationToken: CancellationToken.None);
            }
        }
    }
}