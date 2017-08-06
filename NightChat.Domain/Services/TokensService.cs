using System;
using NightChat.Core.TimeProviders;
using NightChat.Domain.Dto;
using NightChat.Domain.Entities;
using NightChat.Domain.Repositories;

namespace NightChat.Domain.Services
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

        public void AddOrUpdateToken(string userId, TokenData tokenModel)
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

        private static Token GetToken(string userId, TokenData tokenModel)
        {
            return new Token
            {
                UserId = userId,
                Value = tokenModel.AccessToken,
                ExpiredTimestamp = GetTokenExpiredDate(tokenModel)
            };
        }

        private static DateTime GetTokenExpiredDate(TokenData token)
        {
            return TimeProvider.Current.Now + TimeSpan.FromSeconds(token.ExpiresInSeconds);
        }
    }
}