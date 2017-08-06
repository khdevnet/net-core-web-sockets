using NightChat.Domain.Dto;

namespace NightChat.Domain.Services
{
    public interface ITokensService
    {
        void AddOrUpdateToken(string userId, TokenData token);

        bool Validate(string userId);
    }
}