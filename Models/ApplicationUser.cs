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
        [Display(Name = "Cover Art URL")]
        public string? CoverUrl { get; set; }

        // 🧙 Navigation property to link this user to their created characters
        public ICollection<CharacterViewModel> Characters { get; set; } = new List<CharacterViewModel>();

        // 🏆 Boolean to track if the Crimson Wyrm cover is unlocked
        [Display(Name = "Has Crimson Wyrm Cover")]
        public bool HasCrimsonWyrmCover { get; set; }
    }
}
