using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public CategoriesController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("This category isn't exist in database. Try another id.");
            }
            
            return Ok(new
            {
                message = "Found it!",
                data = category
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category newCategory)
        {
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newCategory.CategoryId }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, Category neededCategory)
        {
            if (id != neededCategory.CategoryId)
            {
                return BadRequest("Id is not the same!");
            }

            var willChangeCategory = await _context.Categories.FindAsync(id);

            if (willChangeCategory == null)
            {
                return NotFound("This category isn't exist in database. Try another id.");
            }

            //////////

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This category is updated successfully",
                data = willChangeCategory
            });
        }
    }
}
