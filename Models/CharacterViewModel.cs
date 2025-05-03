using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DnDWebpage.Models
{
    public class CharacterViewModel : IValidatableObject
    {
        public int Id { get; set; }

        // Basic Information
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Background is required.")]
        public string Background { get; set; } = "";

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

        public string? ImageUrl { get; set; } 


        [Required(ErrorMessage = "Level is required.")]
        [Range(1, 20, ErrorMessage = "Level must be between 1 and 20.")]
        public int Level { get; set; } = 1;

        // Race and Subrace
        [Required(ErrorMessage = "Race is required.")]
        public string Race { get; set; } = "";

        public string? Subrace { get; set; }

        // Class and Subclass
        [Required(ErrorMessage = "Class is required.")]
        public string Class { get; set; } = "";

        public string Subclass { get; set; } = "";

        public string LevelLog { get; set; } = "";


        // Combat Stats
        public int? HitPoints { get; set; }
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        // Relationship to User
        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }

        // Custom Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in ValidateSubraceRequirement())
                yield return result;

            foreach (var result in ValidateSubclassRequirement())
                yield return result;

            foreach (var result in ValidateImageRequirement())
                yield return result;
        }


        private IEnumerable<ValidationResult> ValidateSubraceRequirement()
        {
            var racesNeedingSubrace = new[] { "elf", "dwarf", "gnome" };
            if (!string.IsNullOrWhiteSpace(Race) &&
                Array.Exists(racesNeedingSubrace, r => r.Equals(Race, StringComparison.OrdinalIgnoreCase)) &&
                string.IsNullOrWhiteSpace(Subrace))
            {
                yield return new ValidationResult(
                    $"The race '{Race}' requires you to select a subrace.",
                    new[] { nameof(Subrace) });
            }
        }

        private IEnumerable<ValidationResult> ValidateSubclassRequirement()
        {
            var subclassThresholds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "barbarian", 3 }, { "bard", 3 }, { "cleric", 1 },
                { "druid", 2 }, { "fighter", 3 }, { "monk", 3 },
                { "paladin", 3 }, { "ranger", 2 }, { "rogue", 3 },
                { "sorcerer", 1 }, { "warlock", 1 }, { "wizard", 2 }
            };

            if (!string.IsNullOrWhiteSpace(Class) &&
                subclassThresholds.TryGetValue(Class, out int requiredLevel))
            {
                if (Level < requiredLevel && !string.IsNullOrWhiteSpace(Subclass))
                {
                    yield return new ValidationResult(
                        $"{Class} subclasses cannot be chosen before level {requiredLevel}.",
                        new[] { nameof(Subclass) });
                }
                else if (Level >= requiredLevel && string.IsNullOrWhiteSpace(Subclass))
                {
                    yield return new ValidationResult(
                        $"Please select a subclass for {Class} (unlocked at level {requiredLevel}).",
                        new[] { nameof(Subclass) });
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateImageRequirement()
        {
            if ((ImageUpload == null || ImageUpload.Length == 0) && string.IsNullOrWhiteSpace(ImageUrl))
            {
                yield return new ValidationResult(
                    "Please upload an image OR provide a URL.",
                    new[] { nameof(ImageUpload), nameof(ImageUrl) });
            }
        }



    }
}
