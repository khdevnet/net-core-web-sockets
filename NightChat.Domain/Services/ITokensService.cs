namespace NightChat.Domain.Services
{
    public interface ITokensService
    {
        void AddOrUpdateToken(string userId, string accessToken, int expiresInSeconds);

        bool Validate(string userId);
    }
}