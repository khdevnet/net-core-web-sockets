namespace NightChat.Sockets.Infrastructure.Models
{
    public class ChatMessageResponseModel
    {
        public ChatMessageResponseModel(string name, string avatar, string message)
        {
            Avatar = avatar;
            Name = name;
            Message = message;
        }

        public string Avatar { get; }

        public string Name { get; }

        public string Message { get; }
    }
}