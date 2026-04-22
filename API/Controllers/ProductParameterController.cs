using API.DTO.Raquests;
using API.DTO.Requests;
using API.NewDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductParameterController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public ProductParameterController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductParameter>>> GetList()
        {
            var list = await db.ProductParameters.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<ProductParameter>>> Get(int id)
        {
            var obj = await db.ProductParameters.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ProductParameterRequest request)
        {
            var obj = new ProductParameter
            {
                Meaning = request.Meaning,
                ParameterId = request.ParameterId,
                ProductId = request.ProductId,
            };
            db.ProductParameters.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromForm] ProductParameterRequest request)
        {
            var obj = await db.ProductParameters.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();

            obj.Meaning = request.Meaning;
            obj.ParameterId = request.ParameterId;
            obj.ProductId = request.ProductId;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await db.ProductParameters.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            
            db.ProductParameters.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
