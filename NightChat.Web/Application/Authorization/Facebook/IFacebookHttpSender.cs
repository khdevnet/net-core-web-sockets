using System.Net;
using NightChat.Web.Facebook.Models;

namespace NightChat.Web.Facebook
{
    public interface IFacebookHttpSender
    {
        UserInfoModel GetUserDetails(string token);

        TokenModel GetToken(string code);

        HttpStatusCode InspectToken(string token);
    }
}