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
        public IActionResult Index(bool error = false)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return View("Logout", User.Identity.Name);
            }
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var response = await _service.Login(dto);
            if(response == null)
            {
                return RedirectToAction("Index", "Login", new { error = true });
            }
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, response.Username), new Claim(ClaimTypes.Role, response.Role), new Claim(ClaimTypes.NameIdentifier, response.UserId) };
            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            return RedirectToAction("Index", "Home");
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
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
