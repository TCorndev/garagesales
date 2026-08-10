using garagesales.Models;
using garagesales.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garagesales.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageSalesController : ControllerBase
    {
        private readonly Database1Context _dbContext;

        public GarageSalesController(Database1Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetGarageSales()
        {
            var sales = _dbContext.GarageSales.Include(x => x.GarageSaleItems).ToList();
            return Ok(sales);
        }
        [HttpGet("{id}")]
        public IActionResult GetGarageSale(int id)
        {
            var sale = _dbContext.GarageSales.Find(id);

            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }
        [HttpPost]
        public IActionResult AddGarageSale(GarageSaleDto dto)
        {
            var sale = new GarageSale()
            {
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };

            _dbContext.GarageSales.Add(sale);
            _dbContext.SaveChanges();
            return Ok(sale);
        }
    }
}
