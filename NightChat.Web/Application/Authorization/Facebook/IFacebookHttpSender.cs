using System.Net;
using NightChat.Web.Application.Authorization.Facebook.Models;

namespace NightChat.Web.Application.Authorization.Facebook
{
    public interface IFacebookHttpSender
    {
        UserInfoModel GetUserDetails(string token);

        TokenModel GetToken(string code);

        HttpStatusCode InspectToken(string token);
    }
}