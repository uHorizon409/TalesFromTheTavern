using DnDWebpage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace DnDWebpage.Areas.Identity.Pages.Account
{
    public class RetireModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RetireModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public string ProfileImageUrl { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            ProfileImageUrl = user?.ProfileImagePath ?? "/images/icons/user.png";

            // ? Set the avatar for _Layout.cshtml
            ViewData["NavAvatar"] = ProfileImageUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            TempData["LogoutMessage"] = "You passed out on the tavern floor...";
            return LocalRedirect(returnUrl ?? Url.Page("/Index", new { area = "" }));
        }
    }
}
