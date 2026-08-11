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
        //Login page, includes optional error message that will be displayed
        public IActionResult Index(bool error = false, string? message = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return View("Logout", User.Identity.Name);
            }
            ViewBag.Error = error;
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        //Login post function, takes the parameters from the login dto and attempts to login
        public async Task<IActionResult> Login(LoginDto dto, string? error)
        {
            var response = await _service.Login(dto);
            if(response == null)
            {
                return RedirectToAction("Index", "Login", new { error = true, message = "Invalid Login Credentials" });
            }
            //Creates the claims used in the authentication process for the authentication cookie
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, response.Username), new Claim(ClaimTypes.Role, response.Role), new Claim(ClaimTypes.NameIdentifier, response.UserId) };
            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            return RedirectToAction("Index", "Home");
        }
        //Returns the Register page
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //Attempts to create a new user based on the parameters in the dto, redirecting back to the login page with errors if necessary
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { error = true, message = "Something went wrong, please try again" });
            }
            if (dto.Password != dto.Confirm)
            {
                return RedirectToAction("Index", new { error = true, message = "Passwords don't match, please try registering again" });
            }
            try
            {
                await _service.RegisterUser(dto);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", new { error = true, message = "Password isn't strong enough. Password must have a length of at least 6, uppercase and lowercase letters, one number, and one special character" });
            }
        }
        [HttpPost]
        //Logs the user out
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
