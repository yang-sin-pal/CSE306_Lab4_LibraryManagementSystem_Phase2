using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly LibraryManagementDbContext _context;

        public UsersController(LibraryManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("activate")]
        public async Task<IActionResult> Activate(string code)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ActiveCode == code);

            if (user == null)
            {
                return BadRequest("Invalid activation code.");
            }

            user.IsActive = true;

            user.ActiveCode = null;

            await _context.SaveChangesAsync();

            return Ok("Account activated successfully.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            string activeCode = Guid.NewGuid().ToString();

            var user = new User
            {
                Fullname = request.Fullname,
                Email = request.Email,
                Password = hashedPassword,
                Phone = request.Phone,
                Address = request.Address,

                CreatedDate = DateTime.Now,

                ActiveCode = activeCode,

                IsActive = false,
                IsDeleted = false,
                IsLocked = false,

                Status = 1
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Register successfully. Please activate your account.",
                activationCode = activeCode
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            bool isPasswordCorrect =
                BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordCorrect)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                return BadRequest("Account is not activated.");
            }

            if (user.IsLocked)
            {
                return BadRequest("Account is locked.");
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                fullname = user.Fullname,
                email = user.Email
            });
        }

    }
}
