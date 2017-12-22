using NightChat.Web.Application.Extensibility.Authentication.Facebook.Models;

namespace NightChat.Web.Application.Extensibility.Authentication
{
    public interface ICookieAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model, TokenModel token);

        void SignOut();
    }
}