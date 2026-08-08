using Microsoft.AspNetCore.Mvc;

namespace garagesales.Controllers
{
    public class CreateController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
