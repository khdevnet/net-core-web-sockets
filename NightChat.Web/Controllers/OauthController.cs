using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using NightChat.Web.Extensibility.Authentication;
using NightChat.Web.Extensibility.Authentication.Facebook;
using NightChat.Web.Extensibility.Authentication.Facebook.Models;
using NightChat.Web.Extensibility.Providers;

namespace NightChat.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class OauthController : Controller
    {
        private readonly IFacebookLoginUrlProvider facebookLoginUrlProvider;
        private readonly IFacebookAuthentication facebookAuthentication;
        private readonly ICookieAuthenticationService formsAuthenticationService;

        public OauthController(
           IFacebookLoginUrlProvider facebookLoginUrlProvider,
           IFacebookAuthentication facebookAuthentication,
           ICookieAuthenticationService formsAuthenticationService)
        {
            this.facebookLoginUrlProvider = facebookLoginUrlProvider;
            this.facebookAuthentication = facebookAuthentication;
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
                facebookAuthentication.Authorize(codeModel);
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