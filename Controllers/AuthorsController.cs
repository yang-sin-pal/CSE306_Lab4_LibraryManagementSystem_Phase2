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
        private readonly IWebHostEnvironment _environment;

        public AuthorsController(LibraryManagementDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll(string? name)
        {
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a =>
                     (a.FirstName + " " + a.LastName).Contains(name));
            }

            return await query.ToListAsync();
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
        public async Task<IActionResult> Add([FromForm]AuthorCreateDto dto)
        {
            //save avatar
            string? avatarPath = null;

            if (dto.AvatarFile != null)
            {
                var avatarName =
                    Guid.NewGuid()
                    + Path.GetExtension(dto.AvatarFile.FileName);

                var fullPath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "author_avatars",
                        avatarName);

                using var stream =
                    new FileStream(fullPath, FileMode.Create);

                await dto.AvatarFile.CopyToAsync(stream);

                avatarPath =
                    "/author_avatars/" + avatarName;
            }


            _context.Authors.Add(dto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.AuthorId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Author neededAuthor)
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

            willChangeAuthor.FirstName = neededAuthor.FirstName;
            willChangeAuthor.LastName = neededAuthor.LastName;   
            willChangeAuthor.DateOfBirth = neededAuthor.DateOfBirth;
            willChangeAuthor.Biography = neededAuthor.Biography;
            willChangeAuthor.Nationality = neededAuthor.Nationality;
            willChangeAuthor.Avatar = neededAuthor.Avatar;

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

            author.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok("This author is deleted successfully.");
        }
    }
}
