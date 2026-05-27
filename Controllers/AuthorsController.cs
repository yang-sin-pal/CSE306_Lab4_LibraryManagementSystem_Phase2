using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public AuthorsController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll()
        {
            return await _context.Authors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound("This author isn't exist in database. Try another id.");
            }

            return Ok(new
            {
                message = "Found it!",
                data = author
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Author newAuthor)
        {
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newAuthor.AuthorId }, newAuthor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Book neededAuthor)
        {
            if (id != neededAuthor.AuthorId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeAuthor = await _context.Authors.FindAsync(id);

            if (willChangeAuthor == null)
            {
                return NotFound("This author isn't exist in database. Try another id.");
            }

            //////////

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This author is updated successfully",
                data = willChangeAuthor
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound("This author isn't exist in database. Try another id.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return Ok("This author is deleted successfully.");
        }
    }
}
