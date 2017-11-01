using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using NightChat.Web.Application.Authorization;
using NightChat.Web.Application.Authorization.Facebook;
using NightChat.Web.Application.Authorization.Facebook.Models;
using NightChat.Web.Application.Authorization.Facebook.Providers;

namespace NightChat.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class OauthController : Controller
    {
        private readonly IFacebookLoginUrlProvider facebookLoginUrlProvider;
        private readonly IFacebookAuthorization facebookAuthorization;
        private readonly ICookieAuthenticationService formsAuthenticationService;

        public OauthController(
           IFacebookLoginUrlProvider facebookLoginUrlProvider,
           IFacebookAuthorization facebookAuthorization,
           ICookieAuthenticationService formsAuthenticationService)
        {
            this.facebookLoginUrlProvider = facebookLoginUrlProvider;
            this.facebookAuthorization = facebookAuthorization;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        [HttpGet]
        public ActionResult RedirectToDistributor()
        {
            return Redirect(facebookLoginUrlProvider.Get());
        }

        [HttpGet]
        public ActionResult Callback(CodeModel codeModel)
        {
            try
            {
                facebookAuthorization.Authorize(codeModel);
            }
            catch (AuthenticationException)
            {
                return RedirectToAction("NotAuthorized", "Chat");
            }

            return RedirectToAction("Index", "Chat");
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            formsAuthenticationService.SignOut();
            return RedirectToAction("Login", "Chat");
        }
    }
}