using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NightChat.Web.Extensibility.Authentication;
using NightChat.Web.Extensibility.Sockets;
using NightChat.Web.Extensibility.Sockets.Models;

namespace NightChat.Web.Sockets
{
    public class WebSocketProcessingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IEnumerable<ISocketMessageProcessor> socketMessageProcessors;
        private readonly ICookieAuthenticationService cookieAuthenticationService;
        private readonly IWebSocketConnectionMannager webSocketConnectionMannager;
        private readonly IWebSocketHandler webSocketHandler;

        public WebSocketProcessingMiddleware(
            RequestDelegate next,
            IEnumerable<ISocketMessageProcessor> socketMessageProcessors,
            ICookieAuthenticationService cookieAuthenticationService,
            IWebSocketConnectionMannager webSocketConnectionMannager,
            IWebSocketHandler webSocketHandler)
        {
            this.next = next;
            this.socketMessageProcessors = socketMessageProcessors;
            this.cookieAuthenticationService = cookieAuthenticationService;
            this.webSocketConnectionMannager = webSocketConnectionMannager;
            this.webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next.Invoke(context);
                return;
            }

            CancellationToken cancellationToken = context.RequestAborted;
            using (WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                Claim token = context.User.Claims.FirstOrDefault(c => c.Type == Constants.TokenClaimName);
                SocketConnection connection = new SocketConnection(currentSocket);
                if (!webSocketConnectionMannager.TryAdd(token?.Value, new SocketConnection(currentSocket)))
                {
                    await SignOut(token?.Value);
                }
                while (currentSocket.State == WebSocketState.Open)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    string response = await webSocketHandler.ReceiveStringAsync(connection, cancellationToken);
                    if (String.IsNullOrEmpty(response))
                    {
                        if (currentSocket.State != WebSocketState.Open)
                        {
                            break;
                        }

                        continue;
                    }

                    string responseMessage = GetResponseMessage(response);

                    await webSocketHandler.SendMessageToAllAsync(responseMessage, cancellationToken);
                }
                await SignOut(token?.Value);
            }
        }

        private async Task SignOut(string socketId)
        {
            await webSocketConnectionMannager.Remove(socketId);
            cookieAuthenticationService.SignOut();
        }

        private string GetResponseMessage(string request)
        {
            SocketRequestModel requestModel = JsonConvert.DeserializeObject<SocketRequestModel>(request);
            ISocketMessageProcessor processor = socketMessageProcessors.SingleOrDefault(p => p.MessageType == requestModel.MessageType);
            if (processor != null)
            {
                object requestData = JsonConvert.DeserializeObject(requestModel.Data, processor.MessageDataType);
                object result = processor.Process(requestData);
                return JsonConvert.SerializeObject(result);
            }

            return null;
        }
    }
}