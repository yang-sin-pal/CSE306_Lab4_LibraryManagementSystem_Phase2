using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public BooksController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            return Ok(new
            {
                message = "Found it!",
                data = book
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Book newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newBook.BookId }, newBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Book neededBook)
        {
            if (id != neededBook.BookId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeBook = await _context.Books.FindAsync(id);

            if (willChangeBook == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            willChangeBook.Title = neededBook.Title;
            willChangeBook.Description = neededBook.Description;
            willChangeBook.BookCode = neededBook.BookCode;
            willChangeBook.Publisher = neededBook.Publisher;
            willChangeBook.PublishedYear = neededBook.PublishedYear;
            willChangeBook.TotalCopies = neededBook.TotalCopies;
            willChangeBook.AvailableCopies = neededBook.AvailableCopies;
            willChangeBook.Avatar = neededBook.Avatar;
            willChangeBook.Pdf = neededBook.Pdf;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This book is updated successfully",
                data = willChangeBook
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("This book isn't exist in database. Try another id.");
            }

            
            book.IsDeleted = true;
            book.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok("This book is deleted successfully.");
        }

    }
}
