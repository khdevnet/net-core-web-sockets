using System.Net;
using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Facebook
{
    public interface IFacebookHttpSender
    {
        UserInfoModel GetUserDetails(string token);

        TokenModel GetToken(string code);

        HttpStatusCode InspectToken(string token);
    }
}