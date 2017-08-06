using NightChat.Domain.Dto;

namespace NightChat.Domain.Services
{
    public interface IUsersService
    {
        void AddOrUpdateUser(UserData user);
    }
}