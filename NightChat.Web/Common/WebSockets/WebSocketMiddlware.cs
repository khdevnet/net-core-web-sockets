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

namespace NightChat.Web.Common.WebSockets
{
    public class WebSocketMiddleware
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new ConcurrentDictionary<string, WebSocket>();
        private readonly RequestDelegate next;
        private readonly IEnumerable<IWebSoketMessageProcessor> webSoketProcessors;

        public WebSocketMiddleware(RequestDelegate next, IEnumerable<IWebSoketMessageProcessor> webSoketProcessors)
        {
            this.next = next;
            this.webSoketProcessors = webSoketProcessors;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next.Invoke(context);
                return;
            }

            CancellationToken ct = context.RequestAborted;
            using (WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                string socketId = Guid.NewGuid().ToString();

                Sockets.TryAdd(socketId, currentSocket);

                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    string response = await ReceiveStringAsync(currentSocket, ct);
                    if (string.IsNullOrEmpty(response))
                    {
                        if (currentSocket.State != WebSocketState.Open)
                        {
                            break;
                        }

                        continue;
                    }

                    foreach (KeyValuePair<string, WebSocket> socket in Sockets)
                    {
                        if (socket.Value.State != WebSocketState.Open)
                        {
                            continue;
                        }
                        string responseMessage = GetResponseMessage(context, response);

                        await SendStringAsync(socket.Value, responseMessage, ct);
                    }
                }

                WebSocket dummy;
                Sockets.TryRemove(socketId, out dummy);

                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            }
        }

        private string GetResponseMessage(HttpContext context, string request)
        {
            WebSocketRequestModel requestModel = JsonConvert.DeserializeObject<WebSocketRequestModel>(request);
            IWebSoketMessageProcessor processor = webSoketProcessors.SingleOrDefault(p => p.MessageType == requestModel.MessageType);
            if (processor != null)
            {
                object requestData = JsonConvert.DeserializeObject(requestModel.Data, processor.DataType);
                object result = processor.Process(context, requestData);
                return JsonConvert.SerializeObject(result);
            }

            return null;
        }

        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
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

                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}