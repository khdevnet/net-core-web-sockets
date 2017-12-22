using NightChat.Web.Extensibility.Authentication.Facebook.Models;

namespace NightChat.Web.Extensibility.Authentication
{
    public interface ICookieAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model, TokenModel token);

        void SignOut();
    }
}