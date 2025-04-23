using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DnDWebpage.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Bio")]
        [StringLength(250, ErrorMessage = "Bio must be under 250 characters.")]
        public string Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }

        public string CurrentImagePath { get; set; }

        [Display(Name = "Cover Background")]
        public string CoverUrl { get; set; }
    }
}

