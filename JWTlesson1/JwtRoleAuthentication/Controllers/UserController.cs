using JWTlesson1.API.Enums.Models;
using JWTlesson1.API.JwtRoleAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTlesson1.API.JwtRoleAuthentication.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(
                new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Role = Enums.Role.User
                }, request.Password!);
            if (result.Succeeded)
            {
                request.Password = string.Empty;
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var managedUser = await _userManager.FindByEmailAsync(request.Email!);
            if (managedUser is null)
            {
                return BadRequest("User credentials");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
            if (!isPasswordValid)
            {
                return BadRequest("User credentials");
            }
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb is null)
            {
                return Unauthorized();
            }
            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();
            return Ok(new AuthResponse
            {
                Email = userInDb.Email,
                Token = accessToken,
            });
        }
    }
}
