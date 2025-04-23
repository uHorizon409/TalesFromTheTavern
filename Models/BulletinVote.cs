using DnDWebpage.Models;

public class BulletinVote
{
    public int Id { get; set; }

    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }

    public int? BulletinPostId { get; set; }
    public BulletinPost? BulletinPost { get; set; }

    public DateTime VotedAt { get; set; } = DateTime.UtcNow;

    // ✅ New: Track where the token was dropped
    public float X { get; set; }  // Horizontal position in pixels
    public float Y { get; set; }  // Vertical position in pixels
}
