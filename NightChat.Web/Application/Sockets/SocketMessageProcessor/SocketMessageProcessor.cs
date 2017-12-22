using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Domain.Extensibility.Dto;
using NightChat.Domain.Extensibility.Messages;

namespace NightChat.Web.Application.Sockets.SocketMessageProcessor
{
    internal class SocketMessageProcessor : SocketMessageProcessorBase<SendMessageModel, ReceiveMessageModel>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMessageProcessor messageProcessor;

        public SocketMessageProcessor(IHttpContextAccessor httpContextAccessor, IMessageProcessor messageProcessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.messageProcessor = messageProcessor;
        }

        protected override ReceiveMessageModel Process(SendMessageModel request)
        {
            HttpContext context = httpContextAccessor.HttpContext;
            Claim avatar = context.User.Claims.FirstOrDefault(c => c.Type == Constants.AvatarClaimName);
            UserData userData = new UserData { Id = context.User.Identity.Name, Name = context.User.Identity.Name, Avatar = avatar?.Value };
            return messageProcessor.Process(userData, request);
        }
    }
}