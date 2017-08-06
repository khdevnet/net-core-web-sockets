using NightChat.Web.Facebook.Models;

namespace NightChat.Web.Common.Authorization
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookie(UserInfoModel model, TokenModel token);

        void SignOut();
    }
}