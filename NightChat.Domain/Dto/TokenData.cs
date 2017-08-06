namespace NightChat.Domain.Dto
{
    public class TokenData
    {
        public TokenData(string accessToken, int expiresInSeconds)
        {
            AccessToken = accessToken;
            ExpiresInSeconds = expiresInSeconds;
        }

        public string AccessToken { get; }

        public int ExpiresInSeconds { get; }
    }
}