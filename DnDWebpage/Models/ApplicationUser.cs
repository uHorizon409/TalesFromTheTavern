using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DnDWebpage.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ✅ Updated to match what the Razor view expects
        [Display(Name = "Profile Image Path")]
        public string? ProfileImagePath { get; set; }

        // ✅ Navigation property for the user's characters
        public ICollection<CharacterViewModel> Characters { get; set; } = new List<CharacterViewModel>();
        public string? Bio { get; set; }

        public string? CoverUrl { get; set; }


    }
}
