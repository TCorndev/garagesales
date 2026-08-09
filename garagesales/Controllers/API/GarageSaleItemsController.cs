using garagesales.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garagesales.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageSaleItemsController : ControllerBase
    {
        private readonly Database1Context dbContext;

        public GarageSaleItemsController(Database1Context dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetGarageSaleItems(int id) 
        {
            var items = await dbContext.GarageSaleItems.Where(x => x.GarageSaleId == id).ToListAsync();
            return Ok(items);
        }
    }
}
