using Microsoft.AspNetCore.Mvc;
using NightChat.WebApi.Facebook.Authorization;

namespace NightChat.WebApi.Controllers
{
    public class ChatController : Controller
    {
        [FacebookMvcAuthorize]
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