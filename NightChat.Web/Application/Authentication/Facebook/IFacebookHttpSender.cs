using System.Net;
using NightChat.Web.Application.Authentication.Facebook.Models;

namespace NightChat.Web.Application.Authentication.Facebook
{
    public interface IFacebookHttpSender
    {
        UserInfoModel GetUserDetails(string token);

        TokenModel GetToken(string code);

        HttpStatusCode InspectToken(string token);
    }
}