using AspNetCoreChatRoom.Facebook;
using Microsoft.Extensions.Options;
using NightChat.Web.Controllers;

namespace NightChat.Web.Facebook
{
    public class FacebookRedirectUrlProvider : IFacebookRedirectUrlProvider
    {
        private readonly IOptions<FacebookOauthOptions> facebookOauthOptions;

        public FacebookRedirectUrlProvider(IOptions<FacebookOauthOptions> facebookOauthOptions)
        {
            this.facebookOauthOptions = facebookOauthOptions;
        }

        public string Get()
        {
            return $"{facebookOauthOptions.Value.AppUrl}/Oauth/{nameof(OauthController.Callback)}";
        }
    }
}