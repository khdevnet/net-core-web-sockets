using NightChat.WebApi.Facebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NightChat.Web.Common.Authorization;

namespace NightChat.WebApi.Common.Authorization
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public FormsAuthenticationService(IHttpContextAccessor httpContextAccessor)
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