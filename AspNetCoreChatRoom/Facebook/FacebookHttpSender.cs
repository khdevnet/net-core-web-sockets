using System.Collections.Generic;
using System.Net;
using AspNetCoreChatRoom.Common.Http;
using AspNetCoreChatRoom.Facebook;
using Microsoft.Extensions.Options;
using NightChat.WebApi.Common;
using NightChat.WebApi.Facebook.Models;
using Plugin.Http.Extensibility.Dto;
using Plugin.Http.Extensibility.Senders;

namespace NightChat.WebApi.Facebook
{
    internal class FacebookHttpSender : IFacebookHttpSender
    {
        private const string GetMeUrl = "https://graph.facebook.com/v2.3/me";
        private const string GetAcessTokenUrl = "https://graph.facebook.com/v2.9/oauth/access_token";
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
    }
}