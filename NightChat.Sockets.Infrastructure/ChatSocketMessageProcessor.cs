using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Core.Sockets;
using NightChat.Sockets.Infrastructure.Models;

namespace NightChat.Sockets.Infrastructure
{
    internal class ChatSocketMessageProcessor : SocketMessageProcessorBase<ChatMessageRequestModel, ChatMessageResponseModel>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ChatSocketMessageProcessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override ChatMessageResponseModel Process(ChatMessageRequestModel request)
        {
            HttpContext context = httpContextAccessor.HttpContext;
            Claim avatar = context.User.Claims.FirstOrDefault(c => c.Type == "Avatar");
            var responseModel = new ChatMessageResponseModel(context.User.Identity.Name, avatar?.Value, request.Message);
            return responseModel;
        }
    }
}