using Microsoft.AspNetCore.Mvc;

namespace DnDWebpage.Controllers
{
    public class HomeController2 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
