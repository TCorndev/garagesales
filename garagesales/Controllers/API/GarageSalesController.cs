using garagesales.Models;
using garagesales.Models.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

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
        //Gets all Garage Sales with optional filters
        public IActionResult GetGarageSales(string? City = null, string? State = null, DateTime? StartDate = null)
        {
            var query = _dbContext.GarageSales.Include(x => x.GarageSaleItems).AsQueryable();

            if (!string.IsNullOrWhiteSpace(City))
            {
                query = query.Where(x => x.City == City);
            }
            if (!string.IsNullOrWhiteSpace(State))
            {
                query = query.Where(x => x.State == State);
            }
            if (StartDate.HasValue)
            {
                query = query.Where(x => x.StartTime > StartDate.Value);
            }
            var sales = query.ToList();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        //Gets Specific Garage Sale
        public IActionResult GetGarageSale(int id)
        {
            var sale = _dbContext.GarageSales.Include(x => x.GarageSaleItems).FirstOrDefault(x => x.Id == id);

            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }
        [HttpGet("User/{id}")]
        //Gets all Garage Sales associated with a specific User
        public IActionResult GetGarageSale(string id)
        {
            var sales = _dbContext.GarageSales.Where(x => x.UserId == id).ToList();
            return Ok(sales);
        }
        [HttpGet("filters")]
        //Gets all existing Cities and States to be used as filters
        public IActionResult GetFilters()
        {
            FilterModel model = new FilterModel 
            {
                Cities = _dbContext.GarageSales.Select(x => x.City).Distinct().ToList(),
                States = _dbContext.GarageSales.Select(x => x.State).Distinct().ToList(),
            };
            return Ok(model);
        }
        [HttpPost]
        //Adds a Garage Sale by creating a new one based on the dto's parameters
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
                UserId = dto.UserId,
            };

            _dbContext.GarageSales.Add(sale);
            _dbContext.SaveChanges();
            return Ok(sale);
        }
        [HttpDelete("{id}")]
        //Deletes a specific garage sale, also deletes any items associated with that garage sale first
        public IActionResult DeleteGarageSale(int id)
        {
            var sale = _dbContext.GarageSales.Find(id);
            if (sale == null) 
            {
                return NotFound();
            }
            var items = _dbContext.GarageSaleItems.Where(x => x.GarageSaleId == id);
            _dbContext.GarageSaleItems.RemoveRange(items);
            _dbContext.GarageSales.Remove(sale);
            _dbContext.SaveChanges();
            return Ok(sale);
        }
    }
}
