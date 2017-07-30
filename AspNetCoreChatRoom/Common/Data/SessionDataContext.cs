using System.Collections.Generic;
using System.Linq;
using NightChat.WebApi.Common.Data.Models;
using Microsoft.AspNetCore.Http;
using AspNetCoreChatRoom.Common;

namespace NightChat.WebApi.Common.Data
{
    public class SessionDataContext : ISessionDataContext
    {
        private const string UsersKey = "Session_Users_Data";
        private const string TokensKey = "Session_Tokens_Data";
        private readonly IHttpContextAccessor httpContextAccessor;

        public SessionDataContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            Users = GetEntitySet<User>(UsersKey).ToList();
            Tokens = GetEntitySet<Token>(TokensKey).ToList();
        }

        public IList<User> Users { get; }

        public IList<Token> Tokens { get; }

        public void SaveChanges()
        {
            SetEntitySet(UsersKey, Users.ToList());
            SetEntitySet(TokensKey, Tokens.ToList());
        }

        private List<TEntity> GetEntitySet<TEntity>(string key)
        {
            var val = (httpContextAccessor.HttpContext.Session.Get<List<TEntity>>(key) ?? new List<TEntity>()) as List<TEntity>;
            httpContextAccessor.HttpContext.Session.Set<List<TEntity>>(key, val);
            return val;
        }

        private void SetEntitySet<TEntity>(string key, IEnumerable<TEntity> entities)
        {
            httpContextAccessor.HttpContext.Session.Set(key, entities);
        }
    }
}