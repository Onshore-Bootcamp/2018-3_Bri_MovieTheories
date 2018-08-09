using System.Web.Mvc;

namespace Movie_Theories_Project.Controllers
{
    public class HomeController : Controller
    {
        private ActionResult response;

        public ActionResult Index()
        {
            if(Session["Role"] == null)
            {
                response = RedirectToAction("AllMovieTheories", "MovieTheory");
            }
            else
            {
                response = RedirectToAction("AllMovieTheories", "MovieTheory");
            }
            return response;
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