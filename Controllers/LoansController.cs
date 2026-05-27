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
        public async Task<ActionResult<List<Loan>>> GetAll()
        {
            return await _context.Loans.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loan = await _context.Loans.FindAsync(id);

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
        public async Task<IActionResult> Add(Loan newLoan)
        {
            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newLoan.LoanId }, newLoan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Loan neededLoan)
        {
            if (id != neededLoan.LoanId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeLoan = await _context.Loans.FindAsync(id);

            if (willChangeLoan == null)
            {
                return NotFound("This loan isn't exist in database. Try another id.");
            }

            //////////

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This loan is updated successfully",
                data = willChangeLoan
            });
        }
    }
}
