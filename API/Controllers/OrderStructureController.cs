using API.DTO.Raquests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.NewDB;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStructureController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public OrderStructureController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderStructure>>> GetList()
        {
            var list = await db.OrderStructures.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<OrderStructure>>> Get(int id)
        {
            var obj = await db.OrderStructures.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] OrderStructureRequest request)
        {
            var obj = new OrderStructure
            {
                Quantity = request.Quantity,
                Value = request.Value,
                OrderId = request.OrderId,
                ProductId = request.ProductId,
            };
            db.OrderStructures.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        //[Authorize(Roles = "Accountant")]
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
            var obj = await db.OrderStructures.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            db.OrderStructures.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
