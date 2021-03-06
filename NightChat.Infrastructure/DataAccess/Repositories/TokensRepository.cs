﻿using System.Linq;
using AutoMapper;
using NightChat.Domain.Entities;
using NightChat.Domain.Extensibility.Dto;
using NightChat.Domain.Extensibility.Repositories;
using NightChat.Infrastructure.DataAccess.DataContext;

namespace NightChat.Infrastructure.DataAccess.Repositories
{
    internal class TokensRepository : ITokensRepository
    {
        private readonly ISessionDataContext sessionDataContext;

        public TokensRepository(ISessionDataContext sessionDataContext)
        {
            this.sessionDataContext = sessionDataContext;
        }

        public TokenData GetTokenByUserId(string userId)
        {
            Token token = GetTokenEntity(userId);
            return Mapper.Map<TokenData>(token);
        }

        public void Add(TokenData token)
        {
            if (GetTokenByUserId(token.UserId) == null)
            {
                sessionDataContext.Tokens.Add(Mapper.Map<Token>(token));
                sessionDataContext.SaveChanges();
            }
        }

        public void Update(TokenData token)
        {
            Token tokenEntity = GetTokenEntity(token.UserId);
            tokenEntity.Value = tokenEntity.Value;
            tokenEntity.ExpiredTimestamp = tokenEntity.ExpiredTimestamp;
            sessionDataContext.SaveChanges();
        }

        private Token GetTokenEntity(string userId)
        {
            return sessionDataContext.Tokens.SingleOrDefault(t => t.UserId == userId);
        }
    }
}