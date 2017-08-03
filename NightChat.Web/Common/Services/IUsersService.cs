using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Common.Services
{
    public interface IUsersService
    {
        void AddOrUpdateUser(UserInfoModel userModel);
    }
}