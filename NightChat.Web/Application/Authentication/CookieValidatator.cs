using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using NightChat.Web.Application.Authorization.Facebook;

namespace NightChat.Web.Application.Authentication
{
    public static class CookieValidatator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                await context.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
            }

            var facebookHttpSender = context.HttpContext.RequestServices.GetRequiredService<IFacebookHttpSender>();
            var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == Constants.TokenClaimName);

            if (claim != null)
            {
                var statusCode = facebookHttpSender.InspectToken(claim.Value);
                if (statusCode != HttpStatusCode.OK)
                {
                    await context.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
                }
            }
        }
    }
}