namespace NightChat.Web.Common.Chat
{
    public class MessageResponseModel
    {
        public MessageResponseModel(string name, string avatar, string message)
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