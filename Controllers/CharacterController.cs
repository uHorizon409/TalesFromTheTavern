using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Models;
using DnDWebpage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace DnDWebpage.Controllers
{
    public class CharacterController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CharacterController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
            : base(db)
        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

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
            if (string.IsNullOrEmpty(userId)) return NotFound();

            var characters = _db.Characters.Include(c => c.User).Where(c => c.ApplicationUserId == userId).ToList();
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.Username = user?.UserName ?? "Unknown Adventurer";

            return View("UserCharacters", characters);
        }
        public IActionResult Create() => View("CharacterCreator");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterViewModel model)
        {
            Console.WriteLine("🌍 WEB ROOT PATH: " + _webHostEnvironment.WebRootPath);

            try
            {
                // If they uploaded a file, give it a dummy ImageUrl so our custom validator passes...
                if (model.ImageUpload != null && model.ImageUpload.Length > 0 && string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    model.ImageUrl = "temp-placeholder.jpg";

                    // ...and remove any ModelState entry for ImageUrl so the [Required] never fires
                    ModelState.Remove(nameof(model.ImageUrl));
                }

                // Now run validation without that bogus “ImageUrl is required” error
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("❌ Validation errors:");
                    foreach (var entry in ModelState)
                        foreach (var error in entry.Value.Errors)
                            Console.WriteLine($"- {entry.Key}: {error.ErrorMessage}");

                    return View("CharacterCreator", model);
                }

                // Save the uploaded file and override ImageUrl
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(model.ImageUpload.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(fileStream);
                    }

                    model.ImageUrl = "/uploads/" + uniqueFileName;
                    Console.WriteLine($"✅ Image uploaded and saved: {model.ImageUrl}");
                }

                // Finish up
                model.ApplicationUserId = _userManager.GetUserId(User);
                model.HitPoints = HPHelper.GetInitialHP(model);

                _db.Characters.Add(model);
                await _db.SaveChangesAsync();

                Console.WriteLine("✅ Character saved successfully.");
                return RedirectToAction("Confirmation", new { id = model.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Exception during character creation:");
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Could not create character: " + ex.Message);
                return View("CharacterCreator", model);
            }
        }


        public IActionResult Confirmation(int id)
        {
            var character = _db.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();
            ViewBag.Message = "Your character has been successfully created!";
            return View(character);
        }

        public IActionResult Details(int id)
        {
            var character = _db.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == id);
            return character == null ? NotFound() : View(character);
        }

        public IActionResult Edit(int id)
        {
            var character = _db.Characters.Find(id);
            return character == null ? NotFound() : View(character);
        }

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

        public IActionResult Delete(int id)
        {
            var character = _db.Characters.Find(id);
            return character == null ? NotFound() : View(character);
        }

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
            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDelete()
        {
            var allCharacters = _db.Characters.Include(c => c.User).ToList();
            return View("AdminDelete", allCharacters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrepareLevelUp(int id, string? subclass)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
                return NotFound();

            // Subclass unlock requirements
            var subclassLevels = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        { "barbarian", 3 }, { "bard", 3 }, { "cleric", 1 }, { "druid", 2 },
        { "fighter", 3 }, { "monk", 3 }, { "paladin", 3 }, { "ranger", 3 },
        { "rogue", 3 }, { "sorcerer", 1 }, { "warlock", 1 }, { "wizard", 2 }
    };

            var className = character.Class?.ToLower();
            var nextLevel = character.Level + 1;
            var subclassNeeded = subclassLevels.ContainsKey(className) && nextLevel == subclassLevels[className];

            // If subclass is required and not chosen
            if (subclassNeeded && string.IsNullOrWhiteSpace(subclass))
            {
                TempData["SubclassRequired"] = true;
                TempData["PendingCharacterId"] = character.Id;
                TempData["NextLevel"] = nextLevel;
                return RedirectToAction("Index");
            }

            character.Level = nextLevel;

            if (!string.IsNullOrWhiteSpace(subclass))
                character.Subclass = subclass;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleLevelUp([FromBody] SimpleLevelUpRequest data)
        {
            var character = _db.Characters.FirstOrDefault(c => c.Id == data.Id);
            if (character == null) return NotFound();

            character.Level++;

            if (!string.IsNullOrWhiteSpace(data.Subclass) && string.IsNullOrWhiteSpace(character.Subclass))
                character.Subclass = data.Subclass;

            _db.SaveChanges();
            return Json(new { success = true, newLevel = character.Level });
        }

        public class SimpleLevelUpRequest
        {
            public int Id { get; set; }
            public string? Subclass { get; set; }
        }



        public static class HPHelper
        {
            public static int GetInitialHP(CharacterViewModel character)
            {
                int hitDie = GetHitDieValue(character.Class);
                int conMod = GetModifier(character.Constitution);
                return hitDie + conMod;
            }

            private static int GetHitDieValue(string className)
            {
                return className.ToLower() switch
                {
                    "barbarian" => 12,
                    "fighter" or "paladin" or "ranger" => 10,
                    "bard" or "cleric" or "druid" or "monk" or "rogue" or "warlock" => 8,
                    "sorcerer" or "wizard" => 6,
                    _ => 8
                };
            }

            private static int GetModifier(int abilityScore)
            {
                return (abilityScore - 10) / 2;
            }
        }


    }
}
