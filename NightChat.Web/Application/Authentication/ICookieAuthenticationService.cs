using NightChat.Web.Application.Authentication.Facebook.Models;

namespace NightChat.Web.Application.Authentication
{
    public interface ICookieAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model, TokenModel token);

        void SignOut();
    }
}