namespace NightChat.Domain.Dto
{
    public class UserData
    {
        public UserData(string id, string name, string avatar)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
        }

        public string Id { get; }

        public string Name { get; }

        public string Avatar { get; }
    }
}