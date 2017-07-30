using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Ninject;

namespace NightChat.WebApi.Facebook.Authorization
{
    public class FacebookMvcAuthorizeAttribute : AuthorizeAttribute
    {
        [Inject]
        public IFacebookAuthorization FacebookAuthorization { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!FacebookAuthorization.IsAuthorized(HttpContext.Current.User))
            {
                FormsAuthentication.SignOut();
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Chat", action = "NotAuthorized" }));
        }
    }
}