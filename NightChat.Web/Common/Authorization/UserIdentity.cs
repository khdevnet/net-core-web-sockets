using System.Security.Principal;

namespace NightChat.WebApi.Common.Authorization
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(string authenticationType, bool isAuthenticated, string name, string id, string avatar)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
            Id = id;
            Avatar = avatar;
        }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public string Avatar { get; }

        public string Id { get; }
    }
}