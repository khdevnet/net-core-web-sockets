using Microsoft.Extensions.Options;
using NightChat.Web.Authentication.Facebook;
using NightChat.Web.Controllers;
using NightChat.Web.Extensibility.Providers;

namespace NightChat.Web.Providers
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