using Microsoft.AspNetCore.Mvc;

namespace DnDWebpage.Controllers
{
    public class GameController : Controller
    {
        public IActionResult D20BattleGame()
        {
            return View();
        }
    }
}
