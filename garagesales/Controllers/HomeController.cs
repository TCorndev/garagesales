using garagesales.Models;
using garagesales.Models.dto;
using garagesales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace garagesales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GarageSalesService _service;

        public HomeController(ILogger<HomeController> logger, GarageSalesService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _service.GetGarageSales();
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var sale = await _service.GetGarageSale(id);
                return View(sale);
            }
            catch
            {
                return RedirectToAction("Index", "Home");//Update to error eventually
            }
        }
        [Authorize]
        public IActionResult AddItem(int saleid)
        {
            var dto = new GarageSaleItemDto
            {
                GarageSaleId = saleid
            };
            return PartialView("_additem", dto);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(GarageSaleItemDto item)
        {
            await _service.CreateGarageSaleItem(item);
            return RedirectToAction("Details", new { id = item.GarageSaleId });
        }
        [Authorize]
        public async Task<IActionResult> DeleteItem(int saleid, int itemid)
        {
            //Admin bypasses the need to check if sale belongs to user
            if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                await _service.DeleteGarageSaleItem(itemid);
            }
            else
            {
                var sale = await _service.GetGarageSale(saleid);
                if (sale.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    await _service.DeleteGarageSaleItem(itemid);
                }
            }
            return RedirectToAction("Details", new { id = saleid });
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //Admin bypasses the need to check if sale belongs to user
            if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                await _service.DeleteGarageSale(id);
            }
            else
            {
                var sale = await _service.GetGarageSale(id);
                if (sale.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    await _service.DeleteGarageSale(id);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
