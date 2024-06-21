using Microsoft.AspNetCore.Mvc;
using ecommerce.Repository;
using ecommerce.DTO.User;
using BCrypt.Net;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ecommerce.DTO;
namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private UnitOfWork unit;

        public UserController(UnitOfWork unit, IConfiguration configuration)
        {
            _configuration = configuration;
            this.unit = unit;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO request)
        {
            // Check if user already exists
            if (unit.UserRepository.GetFirstByFilter(u => u.Email == request.email) != null)
                return BadRequest("User already exists");


            //// Hash the password and add the user to the store
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);
            unit.UserRepository.Insert(new AppUser
            {
                FirstName = request.firstName,
                LastName = request.lastName,
                Email = request.email,
                PasswordHash = hashedPassword,
                Role = "user"
            });
            unit.SaveChanges();
            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO request)
        {
            var user = unit.UserRepository.GetFirstByFilter(u => u.Email == request.email && u.Role==request.role);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                return Unauthorized("Invalid username or password");
            // Generate JWT token
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [Authorize(Roles ="string")]
        [HttpGet]
        public IActionResult test()
        {
            return Ok("you are authorized");
        }

        [HttpGet("id")]
        [Authorize]
        public IActionResult userid( )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            return Ok(new { id = userIdClaim.Value });
        }

        [HttpPost("checkmail")]
        public IActionResult isMailAvailable([FromBody]EmailDTO emailDTO)
        {
            var user = unit.UserRepository.GetFirstByFilter(u=>u.Email == emailDTO.email.Trim());
            if(user == null)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
