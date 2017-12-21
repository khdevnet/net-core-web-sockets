using System.Linq;
using Microsoft.AspNetCore.Http;
using NightChat.Domain.Dto;
using NightChat.Domain.Messages;

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
            var context = httpContextAccessor.HttpContext;
            var avatar = context.User.Claims.FirstOrDefault(c => c.Type == Constants.AvatarClaimName);
            var userData = new UserData { Id = context.User.Identity.Name, Name = context.User.Identity.Name, Avatar = avatar?.Value };
            return messageProcessor.Process(userData, request);
        }
    }
}