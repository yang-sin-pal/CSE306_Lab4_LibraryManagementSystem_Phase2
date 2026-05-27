using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public UserController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("This user isn't exist in database. Try another id.");
            }

            return Ok(new
            {
                message = "Found it!",
                data = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, User neededUser)
        {
            if (id != neededUser.UserId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeUser = await _context.Users.FindAsync(id);

            if (willChangeUser == null)
            {
                return NotFound("This user isn't exist in database. Try another id.");
            }

            //////////

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This user is updated successfully",
                data = willChangeUser
            });
        }

    }
}
