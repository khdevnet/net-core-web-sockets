using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Web.Common.Authorization;
using NightChat.Web.Common.WebSockets;

namespace NightChat.Web.Common.Chat
{
    internal class ChatWebSocketMessageProcessor : WebSocketMessageProcessorBase<MessageRequestModel, MessageResponseModel>
    {
        protected override MessageResponseModel Process(HttpContext context, MessageRequestModel request)
        {
            Claim avatar = context.User.Claims.FirstOrDefault(c => c.Type == AuthorizationConstants.AvatarClaimName);
            var responseModel = new MessageResponseModel(context.User.Identity.Name, avatar?.Value, request.Message);
            return responseModel;
        }
    }
}