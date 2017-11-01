using NightChat.Domain.Dto;

namespace NightChat.Domain.Messages
{
    public interface IMessageProcessor
    {
        ReceiveMessageModel Process(UserData user, SendMessageModel request);
    }
}