﻿using System;
using NightChat.Core.TimeProviders;
using NightChat.Domain.Dto;
using NightChat.Domain.Repositories;

namespace NightChat.Domain.Services
{
    internal class TokensService : ITokensService
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
            var user = usersRepository.GetUserById(userId);
            if (user != null)
            {
                var token = tokensRepository.GetTokenByUserId(user.Id);

                if (token != null)
                {
                    return DateTime.Compare(token.ExpiredTimestamp, DateTime.Now) > 0;
                }
            }

            return false;
        }

        public void AddOrUpdateToken(string userId, string accessToken, int expiresInSeconds)
        {
            var token = GetToken(userId, accessToken, expiresInSeconds);

            if (tokensRepository.GetTokenByUserId(userId) != null)
            {
                tokensRepository.Update(token);
            }
            else
            {
                tokensRepository.Add(token);
            }
        }

        private static TokenData GetToken(string userId, string accessToken, int expiresInSeconds)
        {
            return new TokenData(userId, accessToken, GetTokenExpiredDate(expiresInSeconds));
        }

        private static DateTime GetTokenExpiredDate(int expiresInSeconds)
        {
            return TimeProvider.Current.Now + TimeSpan.FromSeconds(expiresInSeconds);
        }
    }
}