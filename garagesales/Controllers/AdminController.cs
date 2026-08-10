using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public string Index()
        {
            return "You are an Admin";
        }
    }
}
