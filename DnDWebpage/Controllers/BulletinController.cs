using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DnDWebpage.Data;
using DnDWebpage.Models;
using Microsoft.EntityFrameworkCore;

namespace DnDWebpage.Controllers
{
    [Authorize]
    public class BulletinController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BulletinController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Admin saves new posts and archives the previous ones
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubmitPosts([FromBody] List<BulletinPost> posts)
        {
            var userId = _userManager.GetUserId(User);

            var current = _context.BulletinPosts.Where(p => !p.IsArchived);
            foreach (var p in current)
                p.IsArchived = true;

            foreach (var post in posts)
            {
                post.PostedAt = DateTime.UtcNow;
                post.UserId = userId;
                _context.BulletinPosts.Add(post);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Return all active posts
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetActivePosts()
        {
            var posts = _context.BulletinPosts
                .Where(p => !p.IsArchived)
                .OrderBy(p => p.Type)
                .Select(p => new
                {
                    p.Id,
                    p.Type,
                    p.Title,
                    p.Description
                })
                .ToList();

            return Json(posts);
        }

        // ✅ Optional admin-only view of past bulletins
        [Authorize(Roles = "Admin")]
        public IActionResult ArchiveList()
        {
            var history = _context.BulletinPosts
                .Where(p => p.IsArchived)
                .OrderByDescending(p => p.PostedAt)
                .ToList();

            return View(history);
        }

        // ✅ Store or update vote with token position
        [HttpPost]
        public async Task<IActionResult> CastVote([FromBody] VoteRequest data)
        {
            var userId = _userManager.GetUserId(User);

            var existingVote = await _context.BulletinVotes
                .FirstOrDefaultAsync(v => v.UserId == userId);

            if (existingVote != null)
            {
                existingVote.BulletinPostId = data.PostId;
                existingVote.X = (float)data.X;
                existingVote.Y = (float)data.Y;
            }
            else
            {
                var vote = new BulletinVote
                {
                    UserId = userId,
                    BulletinPostId = data.PostId,
                    X = (float)data.X,
                    Y = (float)data.Y
                };
                _context.BulletinVotes.Add(vote);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Save token position even if not voting
        [HttpPost]
        public async Task<IActionResult> SaveTokenPosition([FromBody] TokenPositionRequest data)
        {
            var userId = _userManager.GetUserId(User);

            var vote = await _context.BulletinVotes
                .FirstOrDefaultAsync(v => v.UserId == userId);

            if (vote != null)
            {
                vote.X = (float)data.X;
                vote.Y = (float)data.Y;
                // Keep their postId as-is (could be null or voted)
            }
            else
            {
                vote = new BulletinVote
                {
                    UserId = userId,
                    BulletinPostId = null, // No vote yet
                    X = (float)data.X,
                    Y = (float)data.Y
                };
                _context.BulletinVotes.Add(vote);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Public access to all current token placements
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVotes()
        {
            var votes = _context.BulletinVotes
                .Include(v => v.User)
                .Select(v => new
                {
                    userId = v.UserId,
                    postId = v.BulletinPostId,
                    x = v.X,
                    y = v.Y,
                    username = v.User.UserName,
                    avatarUrl = v.User.ProfileImagePath
                })
                .ToList();

            return Json(votes);
        }

        // ✅ Return all registered users who should have tokens
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllUsersWithTokens()
        {
            var users = _context.Users
                .Select(u => new
                {
                    userId = u.Id,
                    username = u.UserName,
                    avatarUrl = u.ProfileImagePath
                })
                .ToList();

            return Json(users);
        }
    }

    // ✅ Data transfer object for vote submission
    public class VoteRequest
    {
        public int PostId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    // ✅ Data transfer object for free token movement
    public class TokenPositionRequest
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
