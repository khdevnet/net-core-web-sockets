using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NightChat.Web.Application.Authentication;

namespace NightChat.Web.Application.Sockets
{
    public class WebSocketProcessingMiddleware
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new ConcurrentDictionary<string, WebSocket>();
        private readonly RequestDelegate next;
        private readonly IEnumerable<ISocketMessageProcessor> socketMessageProcessors;
        private readonly ICookieAuthenticationService cookieAuthenticationService;

        public WebSocketProcessingMiddleware(
            RequestDelegate next,
            IEnumerable<ISocketMessageProcessor> socketMessageProcessors,
            ICookieAuthenticationService cookieAuthenticationService)
        {
            this.next = next;
            this.socketMessageProcessors = socketMessageProcessors;
            this.cookieAuthenticationService = cookieAuthenticationService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next.Invoke(context);
                return;
            }

            var ct = context.RequestAborted;
            using (var currentSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                var socketId = Guid.NewGuid().ToString();

                Sockets.TryAdd(socketId, currentSocket);

                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    var response = await ReceiveStringAsync(currentSocket, ct);
                    if (string.IsNullOrEmpty(response))
                    {
                        if (currentSocket.State != WebSocketState.Open)
                        {
                            cookieAuthenticationService.SignOut();
                            break;
                        }

                        continue;
                    }

                    foreach (KeyValuePair<string, WebSocket> socket in Sockets)
                    {
                        if (socket.Value.State != WebSocketState.Open)
                        {
                            cookieAuthenticationService.SignOut();
                            continue;
                        }
                        var responseMessage = GetResponseMessage(response);

                        await SendStringAsync(socket.Value, responseMessage, ct);
                    }
                }

                WebSocket dummy;
                Sockets.TryRemove(socketId, out dummy);

                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            }
        }

        private string GetResponseMessage(string request)
        {
            var requestModel = JsonConvert.DeserializeObject<SocketRequestModel>(request);
            var processor = socketMessageProcessors.SingleOrDefault(p => p.MessageType == requestModel.MessageType);
            if (processor != null)
            {
                var requestData = JsonConvert.DeserializeObject(requestModel.Data, processor.MessageDataType);
                var result = processor.Process(requestData);
                return JsonConvert.SerializeObject(result);
            }

            return null;
        }

        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}