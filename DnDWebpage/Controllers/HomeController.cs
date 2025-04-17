using DnDWebpage.Controllers;
using DnDWebpage.Data;
using DnDWebpage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        : base(context)
    {
        _logger = logger;
    }

    // GET: /
    public IActionResult Index()
    {
        // Just render Views/Home/Index.cshtml again
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
