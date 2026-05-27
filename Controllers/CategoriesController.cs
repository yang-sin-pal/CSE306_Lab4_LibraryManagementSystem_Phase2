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
        public async Task<ActionResult<List<Category>>> GetAll(bool? isActive,string? name)
        {
            var query = _context.Categories.AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category newCategory)
        {
            newCategory.CreatedDate = DateTime.Now;

            newCategory.IsActive = true;

            _context.Categories.Add(newCategory);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newCategory.CategoryId },
                newCategory);
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

            willChangeCategory.Name = neededCategory.Name;

            willChangeCategory.Description = neededCategory.Description;

            willChangeCategory.Avatar = neededCategory.Avatar;

            willChangeCategory.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "This category is updated successfully",
                data = willChangeCategory
            });
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            category.IsActive = !category.IsActive;

            category.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category status updated successfully.",
                isActive = category.IsActive
            });
        }
    }
}
