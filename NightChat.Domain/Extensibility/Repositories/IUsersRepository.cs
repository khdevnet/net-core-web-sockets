using NightChat.Domain.Extensibility.Dto;

namespace NightChat.Domain.Extensibility.Repositories
{
    public interface IUsersRepository
    {
        UserData GetUserById(string id);

        void Add(UserData user);

        void Update(UserData user);
    }
}