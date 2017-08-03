using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightChat.WebApi.Facebook.Authorization;

namespace NightChat.WebApi.Controllers
{
    public class ChatController : Controller
    {
        // [TypeFilter(typeof(FacebookMvcAuthorizeAttribute))]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}