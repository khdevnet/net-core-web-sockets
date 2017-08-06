using System;

namespace NightChat.Domain.Dto
{
    public class TokenData
    {
        public TokenData(string userId, string accessToken, DateTime expiredTimestamp)
        {
            UserId = userId;
            AccessToken = accessToken;
            ExpiredTimestamp = expiredTimestamp;
        }

        public string UserId { get; }

        public string AccessToken { get; }

        public DateTime ExpiredTimestamp { get; }
    }
}