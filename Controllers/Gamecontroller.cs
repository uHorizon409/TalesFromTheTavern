using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DnDWebpage.Models; // Your ApplicationUser namespace
using System.Threading.Tasks;

public class GameController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GameController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult D20BattleGame()
    {
        return View();
    }

    // 🔥 Add Unlock Method
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UnlockCrimsonWyrmCover()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            user.HasCrimsonWyrmCover = true;

            // Optionally set the cover immediately
            if (string.IsNullOrEmpty(user.CoverUrl))
            {
                user.CoverUrl = "/images/covers/crimson-wyrm.gif";
            }

            await _userManager.UpdateAsync(user);

            return Json(new { success = true });
        }

        return Json(new { success = false, message = "User not found." });
    }
}
