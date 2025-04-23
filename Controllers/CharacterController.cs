using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Models;
using DnDWebpage.Data;
using Microsoft.AspNetCore.Authorization;

namespace DnDWebpage.Controllers
{
    public class CharacterController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CharacterController(ApplicationDbContext db, UserManager<ApplicationUser> userManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Character
        public IActionResult Index(string searchTerm, string classFilter, string raceFilter)
        {
            var query = _db.Characters.Include(c => c.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => c.Name.Contains(searchTerm));

            if (!string.IsNullOrEmpty(classFilter))
                query = query.Where(c => c.Class == classFilter);

            if (!string.IsNullOrEmpty(raceFilter))
                query = query.Where(c => c.Race == raceFilter);

            var characters = query.ToList();

            ViewBag.ClassList = _db.Characters.Select(c => c.Class).Distinct().ToList();
            ViewBag.RaceList = _db.Characters.Select(c => c.Race).Distinct().ToList();

            return View(characters);
        }

        public IActionResult UserCharacters(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var characters = _db.Characters
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToList();

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.Username = user?.UserName ?? "Unknown Adventurer";

            return View("UserCharacters", characters);
        }

        // GET: /Character/Create
        public IActionResult Create() => View("CharacterCreator");

        // POST: /Character/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Stay on the character creation form and preserve input
                return View("CharacterCreator", model);
            }

            try
            {
                // Attach user ID
                model.UserId = _userManager.GetUserId(User);

                // Save to database
                _db.Characters.Add(model);
                await _db.SaveChangesAsync();

                // Redirect to confirmation page
                return RedirectToAction("Confirmation", new { id = model.Id });
            }
            catch (Exception ex)
            {
                // Display DB save failure
                ModelState.AddModelError("", "❌ Could not save your character: " + ex.Message);

                // Stay on form with input intact
                return View("CharacterCreator", model);
            }
        }

        // GET: /Character/Confirmation/{id}
        public IActionResult Confirmation(int id)
        {
            var character = _db.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();
            ViewBag.Message = "Your character has been successfully created!";
            return View(character);
        }

        // GET: /Character/Details/{id}
        public IActionResult Details(int id)
        {
            var character = _db.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();
            return View(character);
        }

        // GET: /Character/Edit/{id}
        public IActionResult Edit(int id)
        {
            var character = _db.Characters.Find(id);
            if (character == null) return NotFound();
            return View(character);
        }

        // POST: /Character/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CharacterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Characters.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // ✅ GET: /Character/Delete/{id}
        public IActionResult Delete(int id)
        {
            var character = _db.Characters.Find(id);
            if (character == null)
                return NotFound();

            return View(character); // Returns the delete confirmation view
        }

        // ✅ POST: /Character/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string? returnUrl)
        {
            var character = _db.Characters.Find(id);
            if (character != null)
            {
                _db.Characters.Remove(character);
                _db.SaveChanges();
            }

            // Redirect to the original page if specified, fallback to Character Index
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
        // ✅ GET: /Character/AdminDelete
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDelete()
        {
            var allCharacters = _db.Characters.Include(c => c.User).ToList();
            return View("AdminDelete", allCharacters);
        }
    }
}
