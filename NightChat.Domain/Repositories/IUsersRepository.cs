using NightChat.Domain.Entities;

namespace NightChat.Domain.Repositories
{
    public interface IUsersRepository
    {
        User GetUserById(string id);

        void Add(User user);

        void Update(User user);
    }
}