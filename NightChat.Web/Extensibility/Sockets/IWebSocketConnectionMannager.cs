using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;
using NightChat.Web.Extensibility.Sockets.Models;

namespace NightChat.Web.Extensibility.Sockets
{
    public interface IWebSocketConnectionMannager
    {
        SocketConnection GetSocketById(string id);

        ConcurrentDictionary<string, SocketConnection> GetAll();

        string GetId(SocketConnection socket);

        bool TryAdd(string id, SocketConnection socket);

        Task Remove(string id, WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure);
    }
}