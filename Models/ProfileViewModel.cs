using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DnDWebpage.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Bio")]
        [StringLength(250, ErrorMessage = "Bio must be under 250 characters.")]
        public string Bio { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfileImage { get; set; }

        public string CurrentImagePath { get; set; } = string.Empty;

        [Display(Name = "Cover Background")]
        public string CoverUrl { get; set; } = string.Empty;

        // Indicates whether the Crimson Wyrm cover is unlocked
        public bool HasCrimsonWyrmCover { get; set; }

        // For storing the uploaded profile image path
        public string ProfileImagePath { get; set; } = string.Empty;

        // For changing password
        public PasswordChangeModel PasswordChange { get; set; } = new PasswordChangeModel();

        // Characters owned by the user
        public List<CharacterViewModel> UserCharacters { get; set; } = new List<CharacterViewModel>();
    }
}
