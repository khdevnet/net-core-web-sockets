using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Web.Application.Authentication.Facebook.Models;

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
            var tokenClaim = new Claim(Constants.TokenClaimName, token.AccessToken);
            var avatarClaim = new Claim(Constants.AvatarClaimName, model.Picture.Data.Url);
            var claimsIdentity = new ClaimsIdentity(identity, new[] { tokenClaim, avatarClaim });
           httpContextAccessor.HttpContext.Authentication.SignInAsync(Constants.AuthCookieName, new ClaimsPrincipal(claimsIdentity));
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
        }
    }
}