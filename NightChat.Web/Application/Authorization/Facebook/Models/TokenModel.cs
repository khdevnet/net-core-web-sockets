using Newtonsoft.Json;

namespace NightChat.Web.Application.Authorization.Facebook.Models
{
    public class TokenModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInSeconds { get; set; }
    }
}