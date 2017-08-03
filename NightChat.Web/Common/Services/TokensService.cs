using System;
using NightChat.WebApi.Common.Data.Models;
using NightChat.WebApi.Common.Data.Repositories;
using NightChat.WebApi.Facebook.Models;
using Plugin.Core.Extensibility.TimeProviders;

namespace NightChat.WebApi.Common.Services
{
    public class TokensService : ITokensService
    {
        private readonly IUsersRepository usersRepository;
        private readonly ITokensRepository tokensRepository;

        public TokensService(
            IUsersRepository usersRepository,
            ITokensRepository tokensRepository)
        {
            this.usersRepository = usersRepository;
            this.tokensRepository = tokensRepository;
        }

        public bool Validate(string userId)
        {
            User user = usersRepository.GetUserById(userId);
            if (user != null)
            {
                Token token = tokensRepository.GetTokenByUserId(user.Id);

                if (token != null)
                {
                    return DateTime.Compare(token.ExpiredTimestamp, DateTime.Now) > 0;
                }
            }

            return false;
        }

        public void AddOrUpdateToken(string userId, TokenModel tokenModel)
        {
            if (tokensRepository.GetTokenByUserId(userId) != null)
            {
                tokensRepository.Update(GetToken(userId, tokenModel));
            }
            else
            {
                tokensRepository.Add(GetToken(userId, tokenModel));
            }
        }

        private static Token GetToken(string userId, TokenModel tokenModel)
        {
            return new Token
            {
                UserId = userId,
                Value = tokenModel.AccessToken,
                ExpiredTimestamp = GetTokenExpiredDate(tokenModel)
            };
        }

        private static DateTime GetTokenExpiredDate(TokenModel token)
        {
            return TimeProvider.Current.Now + TimeSpan.FromSeconds(token.ExpiresInSeconds);
        }
    }
}