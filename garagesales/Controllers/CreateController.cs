using garagesales.Models.dto;
using garagesales.Services;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Controllers
{
    public class CreateController : Controller
    {
        private readonly GarageSalesService _service;

        public CreateController(GarageSalesService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> Create(GarageSaleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ":(";
            }
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
