using System.Collections.Generic;
using AspNetCoreChatRoom.Facebook;
using Microsoft.Extensions.Options;
using NightChat.WebApi.Common;

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
            IOptions<FacebookOauthOptions> facebookOauthOptions,
            IUrlProvider urlProvider,
            IFacebookRedirectUrlProvider facebookRedirectUrlProvider)
        {
            this.urlProvider = urlProvider;
            query.Add("client_id", facebookOauthOptions.Value.ClientId);
            query.Add("redirect_uri", facebookRedirectUrlProvider.Get());
        }

        public string Get()
        {
            return urlProvider.GetUrlQuery(OauthUrl, query);
        }
    }
}