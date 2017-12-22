using Microsoft.Extensions.Options;
using NightChat.Web.Application.Extensibility.Authentication.Facebook.Providers;
using NightChat.Web.Controllers;

namespace NightChat.Web.Application.Authentication.Facebook.Providers
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