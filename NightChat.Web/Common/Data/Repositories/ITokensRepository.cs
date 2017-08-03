using NightChat.WebApi.Common.Data.Models;

namespace NightChat.WebApi.Common.Data.Repositories
{
    public interface ITokensRepository
    {
        Token GetTokenByUserId(string id);

        void Add(Token token);

        void Update(Token token);
    }
}