using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NightChat.Web.Application.Extensibility.Authentication;
using NightChat.Web.Application.Extensibility.Authentication.Facebook.Models;

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
            UserIdentity identity = new UserIdentity("Facebook", true, model.Name, model.Id);
            Claim tokenClaim = new Claim(Constants.TokenClaimName, token.AccessToken);
            Claim avatarClaim = new Claim(Constants.AvatarClaimName, model.Picture.Data.Url);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(identity, new[] { tokenClaim, avatarClaim });
            httpContextAccessor.HttpContext.Authentication.SignInAsync(Constants.AuthCookieName, new ClaimsPrincipal(claimsIdentity));
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
        }
    }
}