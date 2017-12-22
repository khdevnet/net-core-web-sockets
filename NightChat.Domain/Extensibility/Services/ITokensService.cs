namespace NightChat.Domain.Extensibility.Services
{
    public interface ITokensService
    {
        void AddOrUpdateToken(string userId, string accessToken, int expiresInSeconds);

        bool Validate(string userId);
    }
}