using LibraryManagementSystem.DTO;
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

        private readonly IWebHostEnvironment _environment;

        public BooksController(LibraryManagementDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAll(string? search, int? categoryId, int? authorId,int page = 1, int pageSize = 10)
        {
            var query = _context.Books
                .Where(b => !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Title.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId)
                    .Include(b => b.Category);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId)
                    .Include(b => b.Author);
            }

            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

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
        public async Task<IActionResult> Add([FromForm] BookCreateDto dto)
        {
            // Save images
            string? imagePath = null;

            if (dto.ImageFile != null)
            {
                var imageName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(dto.ImageFile.FileName);

                var fullPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_images",
                        imageName);

                using var stream =
                    new FileStream(fullPath, FileMode.Create);

                await dto.ImageFile.CopyToAsync(stream);

                imagePath =
                    "/book_images/" + imageName;
            }


            // Save pdfs
            string? pdfPath = null;

            if (dto.PdfFile != null)
            {
                var pdfName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(dto.PdfFile.FileName);

                var fullPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "book_pdfs",
                        pdfName);

                using var stream =
                    new FileStream(fullPath, FileMode.Create);

                await dto.PdfFile.CopyToAsync(stream);

                pdfPath =
                    "/book_pdfs/" + pdfName;
            }
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
