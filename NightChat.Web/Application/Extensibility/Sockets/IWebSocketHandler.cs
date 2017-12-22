using System.Threading;
using System.Threading.Tasks;
using NightChat.Web.Application.Extensibility.Sockets.Models;

namespace NightChat.Web.Application.Extensibility.Sockets
{
    public interface IWebSocketHandler
    {
        void OnConnected(string id, SocketConnection socket);

        Task OnDisconnected(SocketConnection socket);

        Task SendMessageAsync(SocketConnection socket, string message, CancellationToken cancellationToken);

        Task SendMessageToAllAsync(string message, CancellationToken cancellationToken);

        Task<string> ReceiveStringAsync(SocketConnection socketConnection, CancellationToken ct = default(CancellationToken));
    }
}