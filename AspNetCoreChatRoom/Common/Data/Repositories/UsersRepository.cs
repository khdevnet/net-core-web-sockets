using System.Linq;
using NightChat.WebApi.Common.Data.Models;

namespace NightChat.WebApi.Common.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ISessionDataContext sessionDataContext;

        public UsersRepository(ISessionDataContext sessionDataContext)
        {
            this.sessionDataContext = sessionDataContext;
        }

        public void Add(User user)
        {
            if (GetUserById(user.Id) == null)
            {
                sessionDataContext.Users.Add(user);
                sessionDataContext.SaveChanges();
            }
        }

        public void Update(User user)
        {
            User userEntity = GetUserById(user.Id);
            userEntity.Name = user.Name;
            userEntity.Url = user.Url;
            sessionDataContext.SaveChanges();
        }

        public User GetUserById(string id)
        {
            return sessionDataContext.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}