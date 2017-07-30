using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Common.Authorization
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model);

        void SignOut();
    }
}