using System.Security.Authentication;
using NightChat.WebApi.Common.Authorization;
using NightChat.WebApi.Facebook;
using NightChat.WebApi.Facebook.Authorization;
using NightChat.WebApi.Facebook.Models;
using Microsoft.AspNetCore.Mvc;

namespace NightChat.WebApi.Controllers
{
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

        public ActionResult RedirectToDistributor()
        {
            return Redirect(facebookLoginUrlProvider.Get());
        }

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

        public ActionResult SignOut()
        {
            formsAuthenticationService.SignOut();
            return RedirectToAction("Login", "Chat");
        }
    }
}