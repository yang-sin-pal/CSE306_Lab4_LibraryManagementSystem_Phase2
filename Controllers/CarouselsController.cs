using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarouselsController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public CarouselsController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Carousel>>> GetAll()
        {
            return await _context.Carousels.OrderBy(x => x.Order).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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

        [HttpPost]
        public async Task<IActionResult> Add(Carousel newCarousel)
        {
            _context.Carousels.Add(newCarousel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newCarousel.CarouselId }, newCarousel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Carousel neededCarousel)
        {
            if (id != neededCarousel.CarouselId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeCarousel = await _context.Carousels.FindAsync(id);

            if (willChangeCarousel == null)
            {
                return NotFound("This carousel isn't exist in database. Try another id.");
            }

            willChangeCarousel.ImageUrl = neededCarousel.ImageUrl;
            willChangeCarousel.Title = neededCarousel.Title;
            willChangeCarousel.Description = neededCarousel.Description;
            willChangeCarousel.LinkUrl = neededCarousel.LinkUrl;
            willChangeCarousel.IsActive = neededCarousel.IsActive;
            willChangeCarousel.Order = neededCarousel.Order;
            willChangeCarousel.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Carousel is updated successfully",
                data = willChangeCarousel
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);

            if (carousel == null)
            {
                return NotFound("This carousel isn't exist in database. Try another id.");
            }

            _context.Carousels.Remove(carousel);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Carousel is deleted successfully",
                data = carousel
            });
        }

    }
}
