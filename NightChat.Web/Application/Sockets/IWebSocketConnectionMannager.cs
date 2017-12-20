using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace NightChat.Web.Application.Sockets
{
    public interface IWebSocketConnectionMannager
    {
        WebSocket GetSocketById(string id);

        ConcurrentDictionary<string, WebSocket> GetAll();

        string GetId(WebSocket socket);

        bool TryAdd(string id, WebSocket socket);

        Task Remove(string id);
    }
}