using NightChat.Domain.Extensibility.Dto;

namespace NightChat.Domain.Extensibility.Repositories
{
    public interface ITokensRepository
    {
        TokenData GetTokenByUserId(string id);

        void Add(TokenData token);

        void Update(TokenData token);
    }
}