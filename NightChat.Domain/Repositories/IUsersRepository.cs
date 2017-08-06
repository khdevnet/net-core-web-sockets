using NightChat.Domain.Dto;

namespace NightChat.Domain.Repositories
{
    public interface IUsersRepository
    {
        UserData GetUserById(string id);

        void Add(UserData user);

        void Update(UserData user);
    }
}