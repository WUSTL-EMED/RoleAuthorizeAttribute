using System;
using System.Web.Mvc;
using RoleAuthorize.Mvc;

namespace SampleWebApplication.Controllers
{
    [RoleAuthorize(RoleNames = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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