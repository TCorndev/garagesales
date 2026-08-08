using garagesales.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageSalesController : ControllerBase
    {
        private readonly Database1Context dbContext;

        public GarageSalesController(Database1Context dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetGarageSales()
        {
            var sales = dbContext.GarageSales.ToList();

            return Ok(sales);
        }
        [HttpGet("{id}")]
        public IActionResult GetGarageSale(int id)
        {
            var sale = dbContext.GarageSales.FirstOrDefault(x => x.Id == id);

            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }
    }
}
