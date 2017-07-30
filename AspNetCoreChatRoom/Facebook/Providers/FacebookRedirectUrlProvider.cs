using NightChat.WebApi.Controllers;
using Plugin.Core.Extensibility;

namespace NightChat.WebApi.Facebook
{
    public class FacebookRedirectUrlProvider : IFacebookRedirectUrlProvider
    {
        private readonly IAppSettingsProvider appSettingsProvider;

        public FacebookRedirectUrlProvider(IAppSettingsProvider appSettingsProvider)
        {
            this.appSettingsProvider = appSettingsProvider;
        }

        public string Get()
        {
            return $"{appSettingsProvider.GetValue("appUrl")}/Oauth/{nameof(OauthController.Callback)}";
        }
    }
}