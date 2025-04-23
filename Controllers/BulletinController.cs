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

        // ───────────────────────────────────────────────
        // 📬 POST SUBMISSION (ADMIN ONLY)
        // ───────────────────────────────────────────────

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSinglePost([FromBody] BulletinPost post)
        {
            var userId = _userManager.GetUserId(User);
            var existing = await _context.BulletinPosts
                .FirstOrDefaultAsync(p => !p.IsArchived && p.Type == post.Type);

            if (existing != null)
            {
                existing.Title = post.Title;
                existing.Description = post.Description;
                existing.PostedAt = DateTime.UtcNow;
            }
            else
            {
                post.UserId = userId;
                post.PostedAt = DateTime.UtcNow;
                _context.BulletinPosts.Add(post);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ───────────────────────────────────────────────
        // 📥 RETRIEVE POSTS / VOTES / USERS
        // ───────────────────────────────────────────────

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetActivePosts()
        {
            var posts = _context.BulletinPosts
                .Where(p => !p.IsArchived)
                .OrderBy(p => p.Type)
                .Select(p => new { p.Id, p.Type, p.Title, p.Description })
                .ToList();

            return Json(posts);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ArchiveList()
        {
            var history = _context.BulletinPosts
                .Where(p => p.IsArchived)
                .OrderByDescending(p => p.PostedAt)
                .ToList();

            return View(history);
        }

        [HttpPost]
        public async Task<IActionResult> CastVote([FromBody] VoteRequest data)
        {
            var userId = _userManager.GetUserId(User);
            var vote = await _context.BulletinVotes.FirstOrDefaultAsync(v => v.UserId == userId);

            if (vote != null)
            {
                vote.BulletinPostId = data.PostId;
                vote.X = (float)data.X;
                vote.Y = (float)data.Y;
            }
            else
            {
                vote = new BulletinVote
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

        [HttpPost]
        public async Task<IActionResult> SaveTokenPosition([FromBody] TokenPositionRequest data)
        {
            var userId = _userManager.GetUserId(User);
            var vote = await _context.BulletinVotes.FirstOrDefaultAsync(v => v.UserId == userId);

            if (vote != null)
            {
                vote.X = (float)data.X;
                vote.Y = (float)data.Y;
                vote.BulletinPostId = null;
            }
            else
            {
                vote = new BulletinVote
                {
                    UserId = userId,
                    BulletinPostId = null,
                    X = (float)data.X,
                    Y = (float)data.Y
                };
                _context.BulletinVotes.Add(vote);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVotes()
        {
            try
            {
                var votes = _context.BulletinVotes
                    .Include(v => v.User)
                    .Where(v => v.User != null)
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

                var tallies = votes
                    .Where(v => v.postId != null)
                    .GroupBy(v => v.postId)
                    .ToDictionary(g => g.Key.ToString(), g => new
                    {
                        count = g.Count(),
                        voters = g.Select(v => v.username).ToList()
                    });

                return Json(new { tokens = votes, tallies });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, inner = ex.InnerException?.Message });
            }
        }

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

        // ───────────────────────────────────────────────
        // ⚙️ ADMIN CONTROLS
        // ───────────────────────────────────────────────

        [Authorize(Roles = "Admin")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetBoard()
        {
            // 🧹 Archive current posts
            var posts = _context.BulletinPosts.Where(p => !p.IsArchived);
            foreach (var post in posts)
                post.IsArchived = true;

            // 🔄 Reset all user votes to neutral
            var votes = await _context.BulletinVotes.ToListAsync();
            var rand = new Random();
            int spacing = 60;
            int index = 0;

            foreach (var vote in votes)
            {
                vote.BulletinPostId = null;
                vote.X = 50 + (index % 5) * spacing + rand.Next(-10, 10);  // spread X
                vote.Y = 350 + (index / 5) * spacing + rand.Next(-10, 10); // spread Y
                index++;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Board and votes have been reset." });
        }
    }

    // ───────────────────────────────────────────────
    // 📦 REQUEST MODELS
    // ───────────────────────────────────────────────

    public class VoteRequest
    {
        public int PostId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class TokenPositionRequest
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
