using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public ReportsController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("top-borrowed")]
        public async Task<IActionResult> GetTopBorrowedBooks(
            DateTime? fromDate,
            DateTime? toDate,
            int top = 10)
        {
            var query = _context.Loans
                .Include(l => l.Book)
                .AsQueryable();

            // Filter by date range
            if (fromDate.HasValue)
            {
                query = query.Where(l => l.LoanDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= toDate.Value);
            }

            var result = await query
                .GroupBy(l => new
                {
                    l.BookId,
                    l.Book.Title
                })
                .Select(g => new
                {
                    BookId = g.Key.BookId,
                    Title = g.Key.Title,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(top)
                .ToListAsync();

            return Ok(result);
        }
    }
}