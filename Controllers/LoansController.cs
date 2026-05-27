using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public LoansController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Loan>>> GetAll(int? userId, int? status, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(l => l.UserId == userId);
            }

            if (status != null)
            {
                query = query.Where(l => l.Status == status);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(l => l.LoanDate >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= toDate);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.LoanId == id);

            if (loan == null)
            {
                return NotFound("This loan isn't exist in database. Try another id.");
            }

            return Ok(new
            {
                message = "Found it!",
                data = loan
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoanBook(Loan newLoan)
        {
            var checkBook = await _context.Books.FindAsync(newLoan.BookId);

            if (checkBook == null)
            {
                return NotFound("We don't know this book ");
            }

            if (checkBook.AvailableCopies <= 0)
            {
                return BadRequest("Sorry, we don't have any available copies of this book.");
            }

            newLoan.Status = 0;
            newLoan.LoanDate = DateTime.Now;
            _context.Loans.Add(newLoan);

            checkBook.AvailableCopies -= 1; // Decrease available copies when a loan is created

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newLoan.LoanId }, newLoan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReturnBook(int id, Loan neededLoan)
        {
            if (id != neededLoan.LoanId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeLoan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.LoanId == id);

            if (willChangeLoan == null)
            {
                return NotFound("This loan isn't exist in database. Try another id.");
            }

            if (willChangeLoan.Status == 1)
            {
                return BadRequest("This book has already been returned.");
            }

            willChangeLoan.ReturnDate = DateTime.Now;
            willChangeLoan.Status = 1; // 1 means returned
            
            willChangeLoan.Book.AvailableCopies++; // Return a book mean availabel copies + 1

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This loan is updated successfully",
                data = willChangeLoan
            });
        }
    }
}
