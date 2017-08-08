using NightChat.Domain.Dto;
using NightChat.Infrastructure.Models;

namespace NightChat.Domain.Messages
{
    public interface IMessageProcessor
    {
        ReceiveMessageModel Process(UserData user, SendMessageModel request);
    }
}