using NightChat.Domain.Entities;

namespace NightChat.Domain.Repositories
{
    public interface ITokensRepository
    {
        Token GetTokenByUserId(string id);

        void Add(Token token);

        void Update(Token token);
    }
}