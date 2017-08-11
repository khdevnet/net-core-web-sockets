using NightChat.Web.Application.Authorization.Facebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NightChat.Web.Application.Authorization
{
    public class CookieAuthenticationService : ICookieAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void SetAuthCookie(UserInfoModel model, TokenModel token)
        {
            var identity = new UserIdentity("Facebook", true, model.Name, model.Id);
            var tokenClaim = new Claim(AuthorizationConstants.TokenClaimName, token.AccessToken);
            var avatarClaim = new Claim(AuthorizationConstants.AvatarClaimName, model.Picture.Data.Url);
            var claimsIdentity = new ClaimsIdentity(identity, new[] { tokenClaim, avatarClaim });
           httpContextAccessor.HttpContext.Authentication.SignInAsync(AuthorizationConstants.AuthCookieName, new ClaimsPrincipal(claimsIdentity));
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.Authentication.SignOutAsync(AuthorizationConstants.AuthCookieName);
        }
    }
}