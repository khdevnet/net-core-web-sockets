using NightChat.Web.Application.Authorization.Facebook.Models;

namespace NightChat.Web.Application.Authorization
{
    public interface ICookieAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model, TokenModel token);

        void SignOut();
    }
}