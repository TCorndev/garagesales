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
        public async Task<IActionResult> Create(GarageSaleDto dto)
        {
            string? userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid || userid == null)
            {
                return RedirectToAction("Index");
            }
            dto.UserId = userid;
            try
            {
                int id = await _service.CreateGarageSale(dto);
                return RedirectToAction("Details", "Home", new {id = id});
            }
            catch (Exception ex) {
                return RedirectToAction("Index"); //update this later
            }
        }
    }
}
