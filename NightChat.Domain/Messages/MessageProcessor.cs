using NightChat.Domain.Extensibility.Dto;
using NightChat.Domain.Extensibility.Messages;

namespace NightChat.Domain.Messages
{
    internal class MessageProcessor : IMessageProcessor
    {
        public ReceiveMessageModel Process(UserData user, SendMessageModel request)
        {
            return new ReceiveMessageModel(user.Name, user.Avatar, request.Message);
        }
    }
}