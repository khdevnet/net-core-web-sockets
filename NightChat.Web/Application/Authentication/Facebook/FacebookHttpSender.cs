using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Options;
using NightChat.Core.Http;
using NightChat.Core.Http.Dto;
using NightChat.Core.Http.Senders;
using NightChat.Web.Common;
using NightChat.Web.Application.Authentication.Facebook.Models;
using NightChat.Web.Application.Authentication.Facebook.Providers;

namespace NightChat.Web.Application.Authentication.Facebook
{
    internal class FacebookHttpSender : IFacebookHttpSender
    {
        private const string GetMeUrl = "https://graph.facebook.com/v2.3/me";
        private const string GetAcessTokenUrl = "https://graph.facebook.com/v2.9/oauth/access_token";
        private const string InspectTokenUrl = "https://graph.facebook.com/debug_token";
        private readonly IHttpMessageSender httpMessageSender;
        private readonly IUrlProvider urlProvider;
        private readonly FacebookOauthOptions facebookOauthOptions;
        private readonly IFacebookRedirectUrlProvider facebookRedirectUrlProvider;

        public FacebookHttpSender(
            IHttpMessageSender httpMessageSender,
            IUrlProvider urlProvider,
            IOptions<FacebookOauthOptions> facebookOauthOptions,
            IFacebookRedirectUrlProvider facebookRedirectUrlProvider)
        {
            this.httpMessageSender = httpMessageSender;
            this.urlProvider = urlProvider;
            this.facebookOauthOptions = facebookOauthOptions.Value;
            this.facebookRedirectUrlProvider = facebookRedirectUrlProvider;
        }

        public UserInfoModel GetUserDetails(string token)
        {
            HttpMessageSenderResponse response = httpMessageSender.Get(urlProvider.GetUrlQuery(GetMeUrl, GetUserInfoUrlQuery(token)), GetHeader());
            return IsResponseOk(response) ? response.Response.Content.Read<UserInfoModel>() : null;
        }

        public TokenModel GetToken(string code)
        {
            HttpMessageSenderResponse response = httpMessageSender.Get(urlProvider.GetUrlQuery(GetAcessTokenUrl, GetTokenUrlQuery(code)));
            return IsResponseOk(response) ? response.Response.Content.Read<TokenModel>() : null;
        }

        public HttpStatusCode InspectToken(string token)
        {
            HttpMessageSenderResponse response = httpMessageSender.Get(urlProvider.GetUrlQuery(InspectTokenUrl, GetInspectTokenUrlQuery(token)));
            return response.StatusCode.HasValue ? response.StatusCode.Value : HttpStatusCode.BadRequest;
        }

        private static bool IsResponseOk(HttpMessageSenderResponse response)
        {
            return response.HasResponse && response.StatusCode == HttpStatusCode.OK;
        }

        private static Dictionary<string, string> GetUserInfoUrlQuery(string token)
        {
            return new Dictionary<string, string>
            {
                { "fields", "email,name,picture" },
                { "access_token", token }
            };
        }

        private static Header GetHeader()
        {
            return new Header("application/json");
        }

        private Dictionary<string, string> GetTokenUrlQuery(string code)
        {
            return new Dictionary<string, string>
            {
                { "client_id", facebookOauthOptions.ClientId },
                { "redirect_uri", facebookRedirectUrlProvider.Get() },
                { "client_secret", facebookOauthOptions.ClientSecret },
                { "code", code }
            };
        }

        private Dictionary<string, string> GetInspectTokenUrlQuery(string token)
        {
            return new Dictionary<string, string>
            {
                { "access_token", facebookOauthOptions.AppToken },
                { "input_token", token }
            };
        }
    }
}