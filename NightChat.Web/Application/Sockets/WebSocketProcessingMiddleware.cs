using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NightChat.Web.Application.Authentication;
using NightChat.Web.Application.Extensibility.Sockets;
using NightChat.Web.Application.Sockets.Models;

namespace NightChat.Web.Application.Sockets
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
            IWebSocketHandler webSocketHandler,
            IKeepAliveTimer keepAliveTimer)
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

            var cancellationToken = context.RequestAborted;
            using (var currentSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                var token = context.User.Claims.FirstOrDefault(c => c.Type == Constants.TokenClaimName);
                var connection = new SocketConnection(currentSocket);
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

                    var response = await webSocketHandler.ReceiveStringAsync(connection, cancellationToken);
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
    }
}