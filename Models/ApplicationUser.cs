using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DnDWebpage.Models
{
    public class ApplicationUser : IdentityUser
    {
        // 📸 Optional path to profile image
        [Display(Name = "Profile Image Path")]
        public string? ProfileImagePath { get; set; }

        // 🧾 Optional short biography
        public string? Bio { get; set; }

        // 🖼️ Optional cover art URL for profile theming
        public string? CoverUrl { get; set; }

        // 🧙 Navigation property to link this user to their created characters
        public ICollection<CharacterViewModel> Characters { get; set; } = new List<CharacterViewModel>();
    }
}
