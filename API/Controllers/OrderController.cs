using API.DTO.Raquests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.NewDB;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public OrderController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetList()
        {
            var list = await db.Orders.ToListAsync();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Order>>> Get(int id)
        {
            var obj = await db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] OrderRequest request)
        {
            var obj = new Order
            {
                Date = DateTime.Now,
                Status = request.Status,
                UserId = request.UserId,
            };
            db.Orders.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Accountant")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromForm] OrderRequest request)
        {
            var obj = await db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();

            obj.Date = DateTime.Now;
            obj.Status = request.Status;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            List<OrderStructure> structures = await db.OrderStructures.Where(x => x.OrderId == obj.Id).ToListAsync();
            foreach (var structure in structures)
            {
                db.OrderStructures.Remove(structure);
            }
            db.Orders.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
