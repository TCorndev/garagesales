using garagesales.Models.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Gets a list of all users for the admin page
        public async Task<IActionResult> GetUsers()
        {
            var userlist = await _userManager.Users.ToListAsync();
            var dtolist = new List<UserDto>();
            foreach(var user in userlist)
            {
                dtolist.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                });
            }
            return Ok(dtolist);
        }
        [HttpPost]
        //Logins with the parameters from the dto
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
            {
                return Unauthorized();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var id = await _userManager.GetUserIdAsync(user);
            return Ok(new
            {
                Username = dto.Username,
                Role = role,
                UserId = id
            });
        }

        [HttpPost("register")]
        //Registers a new user with the parameters in the dto
        public async Task<IActionResult> RegisterUser(CreateUserDto dto)
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
        [Authorize(Roles = "Admin")]
        [HttpPut]
        //Updates a user to be an Admin or a User, based on what they were before
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            string removedrole = "User";
            if (dto.Role == "User")
            {
                removedrole = "Admin";
            }
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null) {
                return BadRequest();
            }
            await _userManager.AddToRoleAsync(user, dto.Role);
            await _userManager.RemoveFromRoleAsync(user, removedrole);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        //Deletes a User
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            await _userManager.DeleteAsync(user);
            return Ok();
        }
        
    }
}
