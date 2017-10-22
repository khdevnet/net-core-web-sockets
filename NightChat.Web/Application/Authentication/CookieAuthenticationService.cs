using NightChat.Web.Application.Authentication.Facebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NightChat.Web.Application.Authentication
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
            var tokenClaim = new Claim(AuthenticationConstants.TokenClaimName, token.AccessToken);
            var avatarClaim = new Claim(AuthenticationConstants.AvatarClaimName, model.Picture.Data.Url);
            var claimsIdentity = new ClaimsIdentity(identity, new[] { tokenClaim, avatarClaim });
           httpContextAccessor.HttpContext.Authentication.SignInAsync(AuthenticationConstants.AuthCookieName, new ClaimsPrincipal(claimsIdentity));
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.Authentication.SignOutAsync(AuthenticationConstants.AuthCookieName);
        }
    }
}