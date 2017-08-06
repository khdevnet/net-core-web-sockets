using System.Linq;
using NightChat.Domain.Entities;
using NightChat.Domain.Repositories;

namespace NightChat.DataAccess.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly ISessionDataContext sessionDataContext;

        public TokensRepository(ISessionDataContext sessionDataContext)
        {
            this.sessionDataContext = sessionDataContext;
        }

        public Token GetTokenByUserId(string userId)
        {
            return sessionDataContext.Tokens.SingleOrDefault(t => t.UserId == userId);
        }

        public void Add(Token token)
        {
            if (GetTokenByUserId(token.UserId) == null)
            {
                sessionDataContext.Tokens.Add(token);
                sessionDataContext.SaveChanges();
            }
        }

        public void Update(Token token)
        {
            Token tokenEntity = GetTokenByUserId(token.UserId);
            tokenEntity.Value = tokenEntity.Value;
            tokenEntity.ExpiredTimestamp = tokenEntity.ExpiredTimestamp;
            sessionDataContext.SaveChanges();
        }
    }
}