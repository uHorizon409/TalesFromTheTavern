using DnDWebpage.Data;
using DnDWebpage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DnDWebpage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // 🔍 Show all users
        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // ✅ Transfer bulletin posts from Ophec to Shadowmaster
        [HttpPost]
        public async Task<IActionResult> TransferPostsFromOphec123()
        {
            var oldUser = await _userManager.FindByNameAsync("Ophec123");
            var newUser = await _userManager.FindByNameAsync("Ophec");

            if (oldUser == null || newUser == null)
                return NotFound("One or both users not found.");

            var posts = await _context.BulletinPosts
                .Where(p => p.UserId == oldUser.Id)
                .ToListAsync();

            foreach (var post in posts)
            {
                post.UserId = newUser.Id;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "All posts transferred from Ophec123 to Ophec.";
            return RedirectToAction("UserList");
        }

        // ❌ Safely delete Ophec123 after verifying post transfer
        [HttpPost]
        public async Task<IActionResult> DeleteOphec123()
        {
            var user = await _userManager.FindByNameAsync("Ophec123");

            if (user == null)
                return NotFound("Ophec123 not found.");

            var hasPosts = await _context.BulletinPosts.AnyAsync(p => p.UserId == user.Id);
            if (hasPosts)
            {
                TempData["ErrorMessage"] = "Ophec123 still has posts. Transfer them first.";
                return RedirectToAction("UserList");
            }

            await _userManager.DeleteAsync(user);
            TempData["SuccessMessage"] = "Ophec123 deleted.";
            return RedirectToAction("UserList");
        }

        // ❌ Safely delete a user
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Check if user has posts
                var hasPosts = await _context.BulletinPosts.AnyAsync(p => p.UserId == user.Id);
                if (hasPosts)
                {
                    TempData["ErrorMessage"] = "This user still has bulletin posts. Please transfer them before deleting.";
                    return RedirectToAction("UserList");
                }

                await _userManager.DeleteAsync(user);
                TempData["SuccessMessage"] = "User deleted.";
            }

            return RedirectToAction("UserList");
        }

        // 📜 Admin bulletin post editor view
        [HttpGet]
        public IActionResult EditBulletin()
        {
            return View("~/Views/Bulletin/Edit.cshtml"); // Manually specify full path
        }

    }
}