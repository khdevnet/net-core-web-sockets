using System.Collections.Generic;
using NightChat.Domain.Entities;

namespace NightChat.DataAccess.DataContext
{
    internal interface ISessionDataContext
    {
        IList<User> Users { get; }

        IList<Token> Tokens { get; }

        void SaveChanges();
    }
}