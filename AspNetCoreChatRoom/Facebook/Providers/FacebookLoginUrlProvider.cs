using System.Collections.Generic;
using NightChat.WebApi.Common;
using Plugin.Core.Extensibility;

namespace NightChat.WebApi.Facebook
{
    public class FacebookLoginUrlProvider : IFacebookLoginUrlProvider
    {
        private const string OauthUrl = "https://www.facebook.com/v2.9/dialog/oauth";
        private readonly IUrlProvider urlProvider;

        private readonly IDictionary<string, string> query = new Dictionary<string, string>
        {
            { "scope", "email" },
            { "response_type", "code" },
        };

        public FacebookLoginUrlProvider(
            IAppSettingsProvider appSettingsProvider,
            IUrlProvider urlProvider,
            IFacebookRedirectUrlProvider facebookRedirectUrlProvider)
        {
            this.urlProvider = urlProvider;
            query.Add("client_id", appSettingsProvider.GetValue("clientId"));
            query.Add("redirect_uri", facebookRedirectUrlProvider.Get());
        }

        public string Get()
        {
            return urlProvider.GetUrlQuery(OauthUrl, query);
        }
    }
}