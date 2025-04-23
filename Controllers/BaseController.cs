using DnDWebpage.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DnDWebpage.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == userId);

                if (user != null && !string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    ViewBag.NavAvatar = user.ProfileImagePath.Replace("\\", "/");
                }
                else
                {
                    ViewBag.NavAvatar = "/images/icons/user.png";
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
