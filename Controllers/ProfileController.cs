using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Data;
using DnDWebpage.Models;
using System.Threading.Tasks;

namespace DnDWebpage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // 🔍 Public Profile View for other users (read-only)
        [HttpGet("/Profile/View/{userId}")]
        public async Task<IActionResult> View(string userId)
        {
            // Attempt to retrieve the user and include related characters
            var user = await _userManager.Users
                .Include(u => u.Characters) // Make sure Characters navigation property exists
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            // ✅ Add: Logged-in user's avatar for navbar consistency
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ViewBag.NavAvatar = !string.IsNullOrEmpty(currentUser.ProfileImagePath)
                    ? currentUser.ProfileImagePath
                    : "/images/icons/user.png";
            }

            // ✅ Return a read-only view of the user's profile
            return View("ReadOnlyProfile", user);
        }
    }
}
