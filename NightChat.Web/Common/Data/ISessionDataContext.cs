using System.Collections.Generic;
using NightChat.WebApi.Common.Data.Models;

namespace NightChat.WebApi.Common.Data
{
    public interface ISessionDataContext
    {
        IList<User> Users { get; }

        IList<Token> Tokens { get; }

        void SaveChanges();
    }
}