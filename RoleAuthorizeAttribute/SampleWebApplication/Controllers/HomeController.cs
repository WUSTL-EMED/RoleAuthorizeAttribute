using System;
using System.Web.Mvc;
using RoleAuthorize.Config;
using RoleAuthorize.Mvc;

namespace SampleWebApplication.Controllers
{
    [RoleAuthorize(RoleNames = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var roles = RoleConfig.GetRoles("Admin");
            var users = RoleConfig.GetUsers("Admin");
            var defaultAllow = RoleConfig.DefaultAllow;
            var authenticated403 = RoleConfig.Authenticated403;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}