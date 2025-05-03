using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Data;
using DnDWebpage.Models;
using System.Linq;
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

        // 🔍 Public Profile View (read-only)
        [HttpGet("/Profile/View/{userId}")]
        public async Task<IActionResult> View(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Characters)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.NavAvatar = currentUser?.ProfileImagePath ?? "/images/icons/user.png";

            return View("ReadOnlyProfile", user);
        }

        // 🧑‍💻 Editable Profile for logged-in user
        [HttpGet("/Profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.Users
                .Include(u => u.Characters)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
                return RedirectToAction("Login", "Account");

            var profileViewModel = new ProfileViewModel
            {
                Username = user.UserName,
                Bio = user.Bio,
                CurrentImagePath = user.ProfileImagePath,
                CoverUrl = user.CoverUrl,
                HasCrimsonWyrmCover = user.HasCrimsonWyrmCover,
                ProfileImagePath = user.ProfileImagePath,
                UserCharacters = user.Characters.Select(c => new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Background = c.Background,
                    ImageUrl = c.ImageUrl,
                    Level = c.Level,
                    Race = c.Race,
                    Subrace = c.Subrace,
                    Class = c.Class,
                    Subclass = c.Subclass,
                    Strength = c.Strength,
                    Dexterity = c.Dexterity,
                    Constitution = c.Constitution,
                    Intelligence = c.Intelligence,
                    Wisdom = c.Wisdom,
                    Charisma = c.Charisma
                }).ToList(),
                PasswordChange = new PasswordChangeModel()
            };

            return View(profileViewModel);
        }

        // 💾 Save Cover Choice
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveCoverChoice([FromBody] CoverChoiceModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.CoverUrl = model.CoverUrl;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok(new { message = "Cover art saved successfully!" });

            return BadRequest(new { message = "Failed to save cover art." });
        }

        // 💾 Save Profile Updates
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveProfile([FromForm] ProfileViewModel profileModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.Bio = profileModel.Bio;

            if (!string.IsNullOrEmpty(profileModel.ProfileImagePath))
            {
                user.ProfileImagePath = profileModel.ProfileImagePath;
            }

            if (!user.HasCrimsonWyrmCover && profileModel.CoverUrl == "/images/covers/profilebanner-option16.jpg")
            {
                user.HasCrimsonWyrmCover = true;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("EditProfile", "Profile");

            return BadRequest(new { message = "Profile update failed." });
        }

        // 🔑 Change Password
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var changeResult = await _userManager.ChangePasswordAsync(user, model.PasswordChange.OldPassword, model.PasswordChange.NewPassword);

            if (changeResult.Succeeded)
            {
                TempData["Message"] = "Password successfully changed!";
                return RedirectToAction("EditProfile");
            }

            foreach (var error in changeResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Reload characters for view in case of errors
            model.UserCharacters = await _context.Characters
                .Where(c => c.ApplicationUserId == user.Id)
                .Select(c => new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Background = c.Background,
                    ImageUrl = c.ImageUrl,
                    Level = c.Level,
                    Race = c.Race,
                    Subrace = c.Subrace,
                    Class = c.Class,
                    Subclass = c.Subclass,
                    Strength = c.Strength,
                    Dexterity = c.Dexterity,
                    Constitution = c.Constitution,
                    Intelligence = c.Intelligence,
                    Wisdom = c.Wisdom,
                    Charisma = c.Charisma
                }).ToListAsync();

            return View("EditProfile", model);
        }
    }
}
