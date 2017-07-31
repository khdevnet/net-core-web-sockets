using NightChat.WebApi.Facebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NightChat.WebApi.Common.Authorization
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public FormsAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void SetAuthCookie(UserInfoModel model)
        {
            var identity = new UserIdentity("Facebook", true, model.Name, model.Id, model.Picture.Data.Url);
            httpContextAccessor.HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", new ClaimsPrincipal(identity));
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
        }
    }
}