using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Data;
using DnDWebpage.Models;

[ApiController]
[Route("api/[controller]")]
public class BookPagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookPagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPages()
    {
        var pages = await _context.BookPages.OrderBy(p => p.PageNumber).ToListAsync();
        return Ok(pages);
    }

    [HttpPost]
    public async Task<IActionResult> SavePage([FromBody] BookPage page)
    {
        var existingPage = await _context.BookPages.FirstOrDefaultAsync(p => p.PageNumber == page.PageNumber);
        if (existingPage != null)
        {
            existingPage.Title = page.Title;
            existingPage.ContentHTML = page.ContentHTML;
        }
        else
        {
            _context.BookPages.Add(page);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // ✅ DELETE a page by PageNumber
    [HttpDelete("{pageNumber}")]
    public async Task<IActionResult> DeletePage(int pageNumber)
    {
        var page = await _context.BookPages.FirstOrDefaultAsync(p => p.PageNumber == pageNumber);
        if (page == null)
        {
            return NotFound();
        }

        _context.BookPages.Remove(page);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Page {pageNumber} deleted successfully." });
    }
}
