using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Common.Services
{
    public interface ITokensService
    {
        void AddOrUpdateToken(string userId, TokenModel tokenModel);

        bool Validate(string userId);
    }
}