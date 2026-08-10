using garagesales.Models.dto;
using garagesales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace garagesales.Controllers
{
    [Authorize]
    public class CreateController : Controller
    {
        private readonly GarageSalesService _service;

        public CreateController(GarageSalesService service)
        {
            _service = service;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<string> Create(GarageSaleDto dto)
        {
            string? userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid || userid == null)
            {
                return ":(";
            }
            dto.UserId = userid;
            try
            {
                await _service.CreateGarageSale(dto);
            }
            catch (Exception ex) {
                return ex.Message; //update this later
            }
            return "You did it";
        }
    }
}
