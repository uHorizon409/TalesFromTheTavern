namespace DnDWebpage.Models
{
    public class BulletinBoardViewModel
    {
        public List<CharacterViewModel> Characters { get; set; } = new();
        public List<ApplicationUser> Users { get; set; } = new();
    }
}
