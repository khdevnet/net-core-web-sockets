using System.Security.Authentication;
using NightChat.Web.Common.Authorization;
using NightChat.Web.Facebook;
using NightChat.Web.Facebook.Authorization;
using NightChat.Web.Facebook.Models;
using Microsoft.AspNetCore.Mvc;

namespace NightChat.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class OauthController : Controller
    {
        private readonly IFacebookLoginUrlProvider facebookLoginUrlProvider;
        private readonly IFacebookAuthorization facebookAuthorization;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public OauthController(
           IFacebookLoginUrlProvider facebookLoginUrlProvider,
           IFacebookAuthorization facebookAuthorization,
           IFormsAuthenticationService formsAuthenticationService)
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