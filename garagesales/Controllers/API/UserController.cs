using garagesales.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garagesales.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(dto.Username));
            var role = roles.FirstOrDefault();
            return Ok(new
            {
                Username = dto.Username,
                Role = role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDto dto)
        {
            var newuser = new IdentityUser();
            newuser.UserName = dto.Username;
            var result = await _userManager.CreateAsync(newuser, dto.Password);
            if (!result.Succeeded) {
                return BadRequest();
            }
            await _userManager.AddToRoleAsync(newuser, "User");
            return Ok();
        }
    }
}
