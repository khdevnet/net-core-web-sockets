using System.Linq;
using NightChat.Domain.Entities;
using NightChat.Domain.Repositories;

namespace NightChat.DataAccess.Repositories
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