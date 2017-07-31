using AspNetCoreChatRoom.Facebook;
using Microsoft.Extensions.Options;
using NightChat.WebApi.Controllers;
using Plugin.Core.Extensibility;

namespace NightChat.WebApi.Facebook
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