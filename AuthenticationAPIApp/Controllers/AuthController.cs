using AuthenticationAPIApp.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace AuthenticationAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        private static readonly List<User> Users = new List<User>{
        new User {UserName = "user1", Password = "password1", Role = "player", Scope = "b_game"},
        new User {UserName = "user2", Password = "password2", Role = "admin", Scope ="vip_character_personalize"}  };

        [HttpPost("/loginWihtoutJwtMiddlewire")]
        public IActionResult Login([FromBody] Login model)
        {

            // Validate the request
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($" empty data.");
                return BadRequest(ModelState);
            }
            _logger.LogInformation($" Authentication for the user." + " " + model.UserName);
            var user = Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user == null)
            {
                _logger.LogInformation($"The user is not found in the list." + " " + model.UserName);
                return Unauthorized("The user is not found in the list.");
            }
            _logger.LogInformation($" Valid user." + " " + model.UserName);
            return Ok(GenerateToken(user));
        }
        [HttpPost("/loginJwtMiddlewire")]
        public async Task<IActionResult> LoginWithJwtMiddlewire([FromBody] Login model)
        {
            var reposne = Unauthorized();
            _logger.LogInformation($" Authentication for the user." + " " + model.UserName);
            // Get the user from the list of users
            var user = Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);

            // Validate the request
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($" empty data.");
                return BadRequest(ModelState);
            }
            // Return unauthorized if user is not found
            if (user == null)
            {
                _logger.LogInformation($"The user is not found in the list." + " " + model.UserName);
                return Unauthorized("The user is not found in the list.");
            }

            // Authenticate the user
            var claims = new List<Claim>{
                          new Claim(ClaimTypes.Name, user.UserName),
                          new Claim(ClaimTypes.Role, user.Role),
                          new Claim("scope", user.Scope)};
            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("DefaultAuthenticationScheme", principal);
            return Ok(new { user });
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("Scope",user.Scope) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
