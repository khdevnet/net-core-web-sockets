using NightChat.Domain.Extensibility.Dto;

namespace NightChat.Domain.Extensibility.Messages
{
    public interface IMessageProcessor
    {
        ReceiveMessageModel Process(UserData user, SendMessageModel request);
    }
}