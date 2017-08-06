using NightChat.Domain.Dto;

namespace NightChat.Domain.Repositories
{
    public interface ITokensRepository
    {
        TokenData GetTokenByUserId(string id);

        void Add(TokenData token);

        void Update(TokenData token);
    }
}