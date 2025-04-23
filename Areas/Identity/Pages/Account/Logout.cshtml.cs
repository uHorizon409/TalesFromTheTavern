using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using DnDWebpage.Models; // ✅ Make sure this is the namespace of ApplicationUser

namespace DnDWebpage.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [TempData]
        public string? LogoutMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            LogoutMessage = "You passed out on the tavern floor...";
            return LocalRedirect(returnUrl ?? Url.Page("/Index", new { area = "" }));
        }
    }
}

