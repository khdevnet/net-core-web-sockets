using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using NightChat.Web.Application.Extensibility.Authentication.Facebook;

namespace NightChat.Web.Application.Authentication
{
    public static class CookieValidatator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            ClaimsPrincipal userPrincipal = context.Principal;
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                await context.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
            }

            IFacebookHttpSender facebookHttpSender = context.HttpContext.RequestServices.GetRequiredService<IFacebookHttpSender>();
            Claim claim = context.Principal.Claims.FirstOrDefault(c => c.Type == Constants.TokenClaimName);

            if (claim != null)
            {
                HttpStatusCode statusCode = facebookHttpSender.InspectToken(claim.Value);
                if (statusCode != HttpStatusCode.OK)
                {
                    await context.HttpContext.Authentication.SignOutAsync(Constants.AuthCookieName);
                }
            }
        }
    }
}