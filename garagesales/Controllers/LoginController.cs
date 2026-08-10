using garagesales.Models.dto;
using garagesales.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace garagesales.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _service; 
        public LoginController(LoginService service) { 
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> Login(LoginDto dto)
        {
            var response = await _service.Login(dto);
            if(response == null)
            {
                return "Login failed";
            }
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, response.Username), new Claim(ClaimTypes.Role, response.Role) };
            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            return $"You are logged in! :D {User.Identity.Name}";
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> CreateUser(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return "Model state not valid";
            }
            if (dto.Password != dto.Confirm)
            {
                return "Passwords don't match";
            }
            await _service.RegisterUser(dto);
            return "You did it?";
        }
    }
}
