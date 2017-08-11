using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NightChat.Web.Application.Authorization.Facebook;

namespace NightChat.Web.Controllers
{
    public class ChatController : Controller
    {
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
    }
}