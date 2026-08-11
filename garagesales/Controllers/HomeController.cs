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
        private readonly GarageSalesService _saleservice;
        private readonly LoginService _loginservice;

        public HomeController(ILogger<HomeController> logger, GarageSalesService saleservice, LoginService loginservice)
        {
            _logger = logger;
            _saleservice = saleservice;
            _loginservice = loginservice;
        }

        public async Task<IActionResult> Index()
        {
            var filters = await _saleservice.GetFilters();
            return View(filters);
        }
        // Calls partial view with selected filters in place
        public async Task<IActionResult> GarageSaleList(SaleFilterDto? filter)
        {
            var sales = await _saleservice.GetGarageSales(filter);
            return PartialView("_garagesalelist", sales);
        }

        //The Details page
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var sale = await _saleservice.GetGarageSale(id);
                return View(sale);
            }
            catch
            {
                return RedirectToAction("Index", "Home");//Update to error eventually
            }
        }
        //Returns the partial view associated with adding items to the garagesale
        public async Task<IActionResult> AddItem(int saleid)
        {
            var dto = new GarageSaleItemDto
            {
                GarageSaleId = saleid
            };
            return PartialView("_additem", dto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem(GarageSaleItemDto item)
        {
            //Checks to make sure sale belongs to the user before adding the item
            var sale = await _saleservice.GetGarageSale(item.GarageSaleId);
            if (sale.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                await _saleservice.CreateGarageSaleItem(item);
            }
            return RedirectToAction("Details", new { id = item.GarageSaleId });
        }
        [Authorize]
        public async Task<IActionResult> DeleteItem(int saleid, int itemid)
        {
            //Admin bypasses the need to check if sale belongs to user
            if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                await _saleservice.DeleteGarageSaleItem(itemid);
            }
            else
            {
                var sale = await _saleservice.GetGarageSale(saleid);
                if (sale.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    await _saleservice.DeleteGarageSaleItem(itemid);
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
                await _saleservice.DeleteGarageSale(id);
            }
            else
            {
                var sale = await _saleservice.GetGarageSale(id);
                if (sale.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    await _saleservice.DeleteGarageSale(id);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        //The Admin page, pulls users too
        public async Task<IActionResult> Admin()
        {
            return View(await _loginservice.GetUsers());
        }

        [Authorize(Roles = "Admin")]
        //Creates a user DTO based on provided parameters. Name is not used
        public async Task<IActionResult> UpdateRole(string id, string role)
        {
            UserDto update = new UserDto
            {
                Id = id,
                Name = "not used",
                Role = role,

            };
            await _loginservice.UpdateRole(update);
            return RedirectToAction("Admin", "Home");
        }
        [Authorize(Roles = "Admin")]
        //Deletes user, but first deletes any garage sales associated with that user
        public async Task<IActionResult> DeleteUser(string id)
        {
            var usersales = await _saleservice.GetUserGarageSales(id);
            foreach (var sale in usersales)
            {
                await _saleservice.DeleteGarageSale(sale.Id);
            }
            await _loginservice.DeleteUser(id);
            return RedirectToAction("Admin", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
