using DnDWebpage.Models;

public class BulletinPost
{
    public int Id { get; set; }

    public string Type { get; set; } = ""; // e.g. "Bounty", "Rumor", "Wanted"
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    public bool IsArchived { get; set; } = false;

    // Optional: link to admin who posted it
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
