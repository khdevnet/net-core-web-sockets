using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using NightChat.Web.Application.Authentication;
using NightChat.Web.Application.Authentication.Facebook;

namespace NightChat.Web.Application.Authentication
{
    public static class CookieValidatator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            ClaimsPrincipal userPrincipal = context.Principal;
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                await context.HttpContext.Authentication.SignOutAsync(AuthenticationConstants.AuthCookieName);
            }

            var sender = context.HttpContext.RequestServices.GetRequiredService<IFacebookHttpSender>();
            var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == AuthenticationConstants.TokenClaimName);

            if (claim != null)
            {
                var r = sender.InspectToken(claim.Value);
                if (r != System.Net.HttpStatusCode.OK)
                {
                    await context.HttpContext.Authentication.SignOutAsync(AuthenticationConstants.AuthCookieName);
                }
            }
        }
    }
}