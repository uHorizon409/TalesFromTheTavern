namespace DnDWebpage.Models
{
    public class CoverChoiceModel
    {
        public string CoverUrl { get; set; }

        // Add a property to track if the cover art is locked for the user
        public bool IsLocked { get; set; }
    }
}
