using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnDWebpage.Models
{
    public class CharacterViewModel : IValidatableObject
    {
        public int Id { get; set; }

        // Basic Character Information
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Background is required.")]
        public string Background { get; set; } = "";

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImageUrl { get; set; } = "";

        [Required(ErrorMessage = "Level is required.")]
        [Range(1, 20, ErrorMessage = "Level must be between 1 and 20.")]
        public int Level { get; set; } = 1;

        // Race Information
        [Required(ErrorMessage = "Race is required.")]
        public string Race { get; set; } = "";

        // Subrace is conditionally required via custom validation
        public string Subrace { get; set; } = "";

        // Class Information
        [Required(ErrorMessage = "Class is required.")]
        public string Class { get; set; } = "";

        // Subclass is conditionally required based on class and level
        public string Subclass { get; set; } = "";

        // Ability Scores
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        // Relationship to the user who created this character
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Custom validation for conditional Subrace and Subclass requirements
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Races that require a subrace selection
            var racesNeedingSubrace = new[] { "elf", "dwarf", "gnome" };
            if (!string.IsNullOrWhiteSpace(Race)
                && Array.Exists(racesNeedingSubrace, r => r.Equals(Race, StringComparison.OrdinalIgnoreCase))
                && string.IsNullOrWhiteSpace(Subrace))
            {
                yield return new ValidationResult(
                    $"Subrace is required for {Race}.",
                    new[] { nameof(Subrace) }
                );
            }

            // Classes and their level thresholds for subclass selection
            var subclassThresholds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "barbarian", 3 },
                { "bard", 3 },
                { "cleric", 1 },   // Domain at level 1
                { "druid", 2 },    // Circle at level 2
                { "fighter", 3 },  // Martial Archetype at level 3
                { "monk", 3 },     // Tradition at level 3
                { "paladin", 3 },  // Oath at level 3
                { "ranger", 2 },   // Archetype at level 2
                { "rogue", 3 },    // Archetype at level 3
                { "sorcerer", 1 }, // Sorcerous Origin at level 1
                { "warlock", 1 },  // Otherworldly Patron at level 1
                { "wizard", 2 }    // Arcane Tradition at level 2
            };

            if (!string.IsNullOrWhiteSpace(Class)
                && subclassThresholds.TryGetValue(Class, out int threshold))
            {
                if (Level < threshold && !string.IsNullOrWhiteSpace(Subclass))
                {
                    yield return new ValidationResult(
                        $"{Class} subclasses cannot be chosen until level {threshold}.",
                        new[] { nameof(Class), nameof(Level), nameof(Subclass) }
                    );
                }
                else if (Level >= threshold && string.IsNullOrWhiteSpace(Subclass))
                {
                    yield return new ValidationResult(
                        $"Please select a subclass for {Class} (available at level {threshold}).",
                        new[] { nameof(Subclass) }
                    );
                }
            }
        }
    }
}
