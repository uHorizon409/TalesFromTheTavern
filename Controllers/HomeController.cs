using DnDWebpage.Controllers;
using DnDWebpage.Data;
using DnDWebpage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(
        ApplicationDbContext context,
        ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager)
        : base(context)
    {
        _logger = logger;
        _userManager = userManager;
    }

    // ?? Home page
    public async Task<IActionResult> Index()
    {
        var usersWithAvatars = await _userManager.Users
            .Where(u => !string.IsNullOrEmpty(u.ProfileImagePath))
            .ToListAsync();

        var characters = await _context.Characters.ToListAsync();

        var viewModel = new BulletinBoardViewModel
        {
            Users = usersWithAvatars,
            Characters = characters
        };

        return View(viewModel);
    }

    // ?? Placeholder pages for navigation buttons

    [HttpGet("/Spells")]
    public IActionResult Spells()
    {
        return View("Spells");
    }

    [HttpGet("/Campaigns")]
    public IActionResult Campaigns()
    {
        return View("Campaigns");
    }

    [HttpGet("/Bestiary")]
    public IActionResult Bestiary()
    {
        return View("Bestiary");
    }

    [HttpGet("/D20BattleGame")]
    public IActionResult D20BattleGame()
    {
        return View("~/Views/Game/D20BattleGame.cshtml");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
