using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;
using NightChat.Web.Application.Sockets.Models;

namespace NightChat.Web.Application.Extensibility.Sockets
{
    public interface IWebSocketConnectionMannager
    {
        SocketConnection GetSocketById(string id);

        ConcurrentDictionary<string, SocketConnection> GetAll();

        string GetId(SocketConnection socket);

        bool TryAdd(string id, SocketConnection socket);

        Task Remove(string id, WebSocketCloseStatus closeStatus = default(WebSocketCloseStatus));
    }
}