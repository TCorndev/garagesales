using garagesales.Models;
using garagesales.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garagesales.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageSaleItemsController : ControllerBase
    {
        private readonly Database1Context _dbContext;

        public GarageSaleItemsController(Database1Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        //Gets Garage Sale Items based on id
        public async Task<IActionResult> GetGarageSaleItems(int id) 
        {
            var items = await _dbContext.GarageSaleItems.Where(x => x.GarageSaleId == id).ToListAsync();
            return Ok(items);
        }
        [HttpPost]
        //Creates a new Garage Sale Item
        public  IActionResult CreateGarageSaleItem(GarageSaleItemDto dto)
        {
            var item = new GarageSaleItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                GarageSaleId = dto.GarageSaleId,
            };
            _dbContext.GarageSaleItems.Add(item);
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        //Deletes a Garage Sale Item
        public IActionResult DeleteGarageSaleItem(int id)
        {
            var item = _dbContext.GarageSaleItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            _dbContext.GarageSaleItems.Remove(item);
            _dbContext.SaveChanges();
            return Ok(item);
        }
    }
}
