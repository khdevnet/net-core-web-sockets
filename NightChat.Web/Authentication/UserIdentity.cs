using System.Security.Principal;

namespace NightChat.Web.Authentication
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(string authenticationType, bool isAuthenticated, string name, string id)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
            Id = id;
        }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public string Id { get; }
    }
}