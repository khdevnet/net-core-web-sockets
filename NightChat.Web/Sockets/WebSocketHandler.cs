using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NightChat.Web.Extensibility.Sockets;
using NightChat.Web.Extensibility.Sockets.Models;

namespace NightChat.Web.Sockets
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly IWebSocketConnectionMannager webSocketConnectionManager;

        public WebSocketHandler(IWebSocketConnectionMannager webSocketConnectionManager)
        {
            this.webSocketConnectionManager = webSocketConnectionManager;
        }

        public void OnConnected(string id, SocketConnection socket)
        {
            webSocketConnectionManager.TryAdd(id, socket);
        }

        public virtual async Task OnDisconnected(SocketConnection socket)
        {
            await webSocketConnectionManager.Remove(webSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(SocketConnection socket, string message, CancellationToken cancellationToken)
        {
            if (socket.Socket.State != WebSocketState.Open)
            {
                return;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
            await socket.Socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        }

        public async Task SendMessageToAllAsync(string message, CancellationToken cancellationToken)
        {
            foreach (KeyValuePair<string, SocketConnection> pair in webSocketConnectionManager.GetAll())
            {
                if (pair.Value.Socket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(pair.Value, message, cancellationToken);
                }
                else
                {
                    await webSocketConnectionManager.Remove(webSocketConnectionManager.GetId(pair.Value));
                }
            }
        }

        public async Task<string> ReceiveStringAsync(SocketConnection socketConnection, CancellationToken ct = default(CancellationToken))
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            using (MemoryStream ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socketConnection.Socket.ReceiveAsync(buffer, ct);
                    socketConnection.LastSeen = DateTime.Now;
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}