namespace DnDWebpage.Models
{
    public class Spell
    {
        public int Id { get; set; }  // Primary key
        public string Name { get; set; }  // Spell Name (e.g., Fireball)
        public string School { get; set; }  // School of magic (Evocation, Abjuration, etc.)
        public int Level { get; set; }  // 0 for Cantrip, 1 for 1st-level, etc.
        public string Description { get; set; }  // Short description of the spell
        public string Classes { get; set; }  // Comma-separated classes that can use it (e.g., "Wizard, Sorcerer")
    }
}
