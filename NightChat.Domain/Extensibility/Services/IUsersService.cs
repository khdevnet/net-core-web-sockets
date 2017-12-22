using NightChat.Domain.Extensibility.Dto;

namespace NightChat.Domain.Extensibility.Services
{
    public interface IUsersService
    {
        void AddOrUpdateUser(UserData user);
    }
}