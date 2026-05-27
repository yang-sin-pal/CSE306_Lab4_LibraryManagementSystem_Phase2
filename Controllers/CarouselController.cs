using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarouselController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public CarouselController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Carousel>>> GetAll()
        {
            return await _context.Carousels.OrderBy(x => x.Order).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound("This carousel isn't exist in database. Try another id.");
            }
            return Ok(new
            {
                message = "Found it!",
                data = carousel
            });
        }
    }
}
