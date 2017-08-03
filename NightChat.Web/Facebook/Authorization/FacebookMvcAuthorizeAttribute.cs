using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NightChat.WebApi.Common.Authorization;

namespace NightChat.WebApi.Facebook.Authorization
{
    public class FacebookMvcAuthorizeAttribute : ActionFilterAttribute
    {
        public FacebookMvcAuthorizeAttribute(IFacebookAuthorization facebookAuthorization, IFormsAuthenticationService formsAuthenticationService)
        {
            FacebookAuthorization = facebookAuthorization;
            FormsAuthenticationService = formsAuthenticationService;
        }

        public IFacebookAuthorization FacebookAuthorization { get; }

        public IFormsAuthenticationService FormsAuthenticationService { get;}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!FacebookAuthorization.IsAuthorized(filterContext.HttpContext.User))
            {
                FormsAuthenticationService.SignOut();
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Chat", action = "NotAuthorized" }));
        }
    }
}