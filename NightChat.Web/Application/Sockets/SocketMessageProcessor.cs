using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Domain.Dto;
using NightChat.Infrastructure.Models;
using NightChat.Core.Sockets;
using NightChat.Domain.Messages;

namespace NightChat.Infrastructure
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
            Claim avatar = context.User.Claims.FirstOrDefault(c => c.Type == "Avatar");
            var userData = new UserData(context.User.Identity.Name, context.User.Identity.Name, avatar?.Value);
            return messageProcessor.Process(userData, request);
        }
    }
}