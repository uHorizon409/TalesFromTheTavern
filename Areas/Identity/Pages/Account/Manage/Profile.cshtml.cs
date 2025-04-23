using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DnDWebpage.Models;
using DnDWebpage.Data;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DnDWebpage.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [BindProperty(SupportsGet = false)]
        public ChangePasswordModel PasswordChange { get; set; } = new ChangePasswordModel();

        public List<CharacterViewModel> UserCharacters { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "Bio")]
            [MaxLength(200, ErrorMessage = "Bio must be under 200 characters.")]
            public string? Bio { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile? ProfileImage { get; set; }

            public string? CurrentImagePath { get; set; }

            [Display(Name = "Cover Background")]
            public string? CoverUrl { get; set; }
        }

        public class ChangePasswordModel
        {
            [ValidateNever]
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string OldPassword { get; set; }

            [ValidateNever]
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }

            [ValidateNever]
            [Required]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "Passwords must match.")]
            [Display(Name = "Confirm New Password")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            Input.Bio = user.Bio;
            Input.CurrentImagePath = user.ProfileImagePath ?? "/uploads/default-avatar.png";
            Input.CoverUrl = user.CoverUrl;

            // ?? Load this user's characters
            UserCharacters = await _context.Characters
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            // Pass avatar image path to layout using ViewData
            ViewData["NavAvatar"] = Input.CurrentImagePath;

            // Set the active page for the navbar
            ViewData["ActivePage"] = "Profile";  // Set this for Profile page

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveImageAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(user.ProfileImagePath))
            {
                var filePath = Path.Combine(_environment.WebRootPath, "profile-icons", user.ProfileImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                user.ProfileImagePath = null;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Bio = Input.Bio?.Trim();

            if (!string.IsNullOrWhiteSpace(Input.CoverUrl))
            {
                user.CoverUrl = Input.CoverUrl;
            }

            if (Input.ProfileImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(Input.ProfileImage.FileName);
                var fileName = user.Id + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImagePath = "/uploads/" + fileName;
            }
            else if (!string.IsNullOrWhiteSpace(Input.CurrentImagePath))
            {
                user.ProfileImagePath = Input.CurrentImagePath;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = "Profile updated!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, PasswordChange.OldPassword, PasswordChange.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = "Password successfully changed!";
            return RedirectToPage();
        }
    }
}
