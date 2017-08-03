using NightChat.WebApi.Common.Data.Models;

namespace NightChat.WebApi.Common.Data.Repositories
{
    public interface IUsersRepository
    {
        User GetUserById(string id);

        void Add(User user);

        void Update(User user);
    }
}