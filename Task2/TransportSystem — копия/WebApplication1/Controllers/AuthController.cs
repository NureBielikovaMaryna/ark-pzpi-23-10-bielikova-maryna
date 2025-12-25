using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

   

        // рег
        [HttpPost("register")]
        
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return BadRequest("Користувач з такою поштою є!");
            }

            
            var newUser = new User
            {
                Name = name,
                Email = email,
                Password = password,
                IsAdmin = false 
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Реєстрація успішна!", userId = newUser.Id });
        }




        // вх
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return Unauthorized("Неправильна пошта або пароль");
            }

            // роли
            string role = (email == "admin@test.com") ? "Admin" : "Client";

            return Ok(new
            {
                message = $"Вітаю, {user.Name}!",
                userId = user.Id,
                role = role
            });
        }
    }
}